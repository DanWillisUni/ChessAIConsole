using ConsoleChess.AI.Model;
using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleChess.AI
{
    public class ComputerBase
    {
        public ComputerBase(OpeningFileStructure openings, int maxDepth = 3, int maxWidth=5, int maxDepthOfCounter=1)
        {
            this.openings = openings;
            this.maxDepth = maxDepth;
            this.maxWidth = maxWidth;
            this.maxDepthOfCounter = maxDepthOfCounter;
        }
        public int maxDepth { get; set; }
        public int maxWidth { get; set; }
        public int maxDepthOfCounter {  get; set; }
        public OpeningFileStructure openings {  get; set; }

        private readonly object moveLock = new object();//mutex object for multi threading
        private readonly Dictionary<Char, Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 1 }, { 'B', 3 }, { 'N', 3 }, { 'R', 5 }, { 'Q', 9 }, { 'K', 200 } };
        public Move getMove(Board b, bool isWhite)
        {
            b.print(!isWhite);
            if (b.pastMoves.Count <= 6)
            {
                Console.WriteLine("Searching openings...");
                var lines = File.ReadAllLines(this.openings.getFullOpenings(isWhite));
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var lineSplit = line.Split("!");
                    if (b.printAsString() == lineSplit[0])
                    {
                        Console.WriteLine("Match");
                        List<Move> r = JsonSerializer.Deserialize<List<Move>>(lineSplit[1]);
                        Random random = new Random();
                        return r[random.Next(r.Count)];
                    }
                }
            }
            return calculateMove(b, isWhite);
        }  
        
        private MoveResult getBestCounter(Move currentMove, Board b, bool isWhite, int depthLeft, bool printDebug = false)
        {
            double lowestScore = int.MaxValue;
            Board lowestCounterBoard = null;
            Board copy = b.DeepCopy();
            copy.makeMove(currentMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            if (printDebug) { Console.WriteLine("All counters:"); }
            List<MoveResult> bestCounters = new List<MoveResult>();
            foreach (Move counterMove in allCounters)
            {
                if (printDebug) { Console.WriteLine(counterMove.ToString()); }
                Board endOfCounterBoard = copy.DeepCopy();
                if (depthLeft > 0)
                {
                    MoveResult moveResultDepthTwo = getBestCounter(counterMove, endOfCounterBoard, !isWhite, depthLeft - 1);
                    bestCounters.Add(new MoveResult(currentMove, moveResultDepthTwo.score, moveResultDepthTwo.boardAfterMove));
                }
                else
                {
                    endOfCounterBoard.makeMove(counterMove);
                    if (printDebug) { Console.WriteLine("Board: " + endOfCounterBoard.printAsString()); }
                    double currentScore = getScore(endOfCounterBoard, isWhite);
                    if (currentScore < lowestScore)
                    {
                        lowestScore = currentScore;
                        lowestCounterBoard = endOfCounterBoard;
                    }
                }
            }
            if (bestCounters.Count > 0) { return prune(bestCounters, true)[0]; }
            return new MoveResult(currentMove, lowestScore, lowestCounterBoard);
        }
        #region scoring
        private double getScore(Board b, bool isWhite)
        {
            double r = 0;
            int forwardMultiplyer = isWhite ? 1 : -1;
            char isWhiteChar = isWhite ? 'W' : 'B';

            foreach(IPieces p in b.allPeices)
            {
                int value = getPeiceValue(p);
                r = (p.isWhite == isWhite) ? r + value : r - value;
            }

            // number of possible moves
            List<Move> ourMoves = b.getAllMoves(isWhite);
            List<Move> oppMoves = b.getAllMoves(!isWhite);
            int moveNumberDiff = ourMoves.Count - oppMoves.Count;
            r += moveNumberDiff * 0.1;

            r += 0.1 * getValueOfThreatening(b, ourMoves);
            r -= 0.1 * getValueOfThreatening(b, oppMoves);

            for (int x = 0; x <= 7; x++)
            {
                int ourPawnsPerColumn = 0;
                int opponentsPawnsPerColumn = 0;
                for(int y = 0; y <= 7; y++)
                {
                    if (b.layout[x, y] != null)
                    {
                        if (b.layout[x, y].ToUpper()[1] == 'P')
                        {
                            //doubled pawn
                            if (b.layout[x, y].ToUpper()[0] == isWhiteChar)
                            {
                                ourPawnsPerColumn++;
                                if (Board.isOnBoard(x, y + forwardMultiplyer))
                                {
                                    if (b.layout[x, y + forwardMultiplyer] != null)
                                    {
                                        r += 0.5;
                                    }
                                }
                            }
                            else
                            {
                                opponentsPawnsPerColumn++;
                                if (Board.isOnBoard(x, y - forwardMultiplyer))
                                {
                                    if (b.layout[x, y - forwardMultiplyer] != null)
                                    {
                                        r -= 0.5;
                                    }
                                }
                            }

                        }
                    }
                }
                if (ourPawnsPerColumn > 1)
                {
                    r += 0.5;
                }
                if (opponentsPawnsPerColumn > 1)
                {
                    r -= 0.5;
                }
            }
            
            // protected pieces value
            // threatened peices
            // threatened uprotected pieces
            // if game is nearly over, number of king moves avialable

            //-0.5(D - D' + S-S' + I - I')
            //D, S, I = doubled, blocked and isolated pawns
            return Math.Round(r, 3, MidpointRounding.AwayFromZero);
        }
        private int getPeiceValue(IPieces p)
        {
            char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
            return peiceValues[type];
        }

        private double getValueOfThreatening(Board b, List<Move> moves)
        {
            double r = 0;
            foreach (Move move in moves)
            {
                IPieces taken = b.allPeices.Where(o => o.location.Equals(move.toLocation)).Select(o => o).FirstOrDefault();
                if (taken != null)
                {
                    r += getPeiceValue(taken);
                }
            }
            return r;
        }

        #endregion
        private Move calculateMove(Board b, bool isWhite)
        {
            Console.WriteLine("Calculating...");

            List<Move> all = b.getAllMoves(isWhite);
            if (all.Count == 1)
            {
                return all[0];
            }
            List<MoveResult> results = new List<MoveResult>();
            Parallel.For<List<MoveResult>>(0, all.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
            {
                threadResults.Add(getMoveFirstIteration(all[(int)i], b, isWhite));
                return threadResults;//return the thread results
            },
            (threadResults) => {
                lock (moveLock)//lock the list
                {
                    results.AddRange(threadResults);//add the thread results to the results list
                }
            });

            foreach (MoveResult result in results) 
            {
                Console.WriteLine(result.ToString());
            }

            List<MoveResult> r = prune(results, true);

            Console.WriteLine("Highest results");
            foreach (MoveResult result in r)
            {
                Console.WriteLine(result.ToString());
            }

            Random random = new Random();
            return r[random.Next(r.Count)].originalMove;
        }

        private MoveResult getMoveFirstIteration(Move firstMove, Board b, bool isWhite)
        {
            MoveResult bestFirstCounter = getBestCounter(firstMove, b, isWhite, maxDepthOfCounter);

            if (maxDepth == 0)
            {
                return bestFirstCounter;
            }
            MoveResult mr = getMoveIteration(bestFirstCounter.boardAfterMove.DeepCopy(), isWhite, maxDepth - 1);
            return new MoveResult(firstMove, mr.score, mr.boardAfterMove);
        }

        private MoveResult getMoveIteration(Board b, bool isWhite, int depthLeft)
        {
            //get counters
            List<Move> allMoves = b.getAllMoves(isWhite);
            List<MoveResult> bestCounters = new List<MoveResult>();
            foreach (Move move in allMoves)
            {
                bestCounters.Add(getBestCounter(move, b.DeepCopy(), isWhite, maxDepthOfCounter));
            }

            if (depthLeft == 0)
            {
                List<MoveResult> pruned = prune(bestCounters, true);
                return pruned[0];
            }
            else
            {
                List<MoveResult> prunedResults = prune(bestCounters);

                //recursion

                List<MoveResult> moveResultsSecond = new List<MoveResult>();
                foreach (MoveResult move in prunedResults)
                {
                    moveResultsSecond.Add(getMoveIteration(move.boardAfterMove, isWhite, depthLeft - 1));
                }

                return prune(moveResultsSecond, true)[0];
            }
        }
        private List<MoveResult> prune(List<MoveResult> all, bool getHighest = false)
        {
            if(all.Count > this.maxWidth)
            {
                List<MoveResult> ordered = all.OrderByDescending(o => o.score).ToList();
                double scoreToBeat = ordered[0].score;
                if (!getHighest)
                {
                    double maxWidthScore = ordered[this.maxWidth].score;
                    double deivationScore = (ordered[0].score - Math.Round(0.1 * Math.Abs(ordered[0].score)));
                    scoreToBeat = Math.Max(maxWidthScore, deivationScore);
                }
                using (IEnumerator<MoveResult> resultsEnumerator = ordered.GetEnumerator())
                {
                    List<MoveResult> toRGT = new List<MoveResult>();
                    List<MoveResult> toREQ = new List<MoveResult>();
                    while (resultsEnumerator.MoveNext())
                    {
                        MoveResult result = resultsEnumerator.Current;
                        if (result.score > scoreToBeat)
                        {
                            toRGT.Add(result);
                        }
                        else if (result.score == scoreToBeat)
                        {
                            toREQ.Add(result);
                        }
                    }

                    if (getHighest)
                    {
                        return toREQ;
                    }

                    Random random = new Random();
                    if (toRGT.Count >= this.maxWidth)
                    {
                        return toRGT.OrderBy(x => random.Next()).Take(this.maxWidth).ToList();
                    }
                    else if (toRGT.Count + toREQ.Count >= this.maxWidth)
                    {
                        List<MoveResult> itemsToAdd = toREQ.OrderBy(x => random.Next()).Take(this.maxWidth - toRGT.Count).ToList();
                        List<MoveResult> toR = [.. toRGT, .. itemsToAdd];
                        return toR;
                    }
                    else
                    {
                        List<MoveResult> toR = [.. toRGT, .. toREQ];
                        return toR;
                    }                    
                }
            }
            else
            {
                return all;
            }
        }
    }
}
