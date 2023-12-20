using ConsoleChess.AI.Model;
using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
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
        public ComputerBase(OpeningFileStructure openings, int maxDepth = 4, int maxWidth=5)
        {
            this.openings = openings;
            this.maxDepth = maxDepth;
            this.maxWidth = maxWidth;
        }
        public int maxDepth { get; set; }
        public int maxWidth { get; set; }
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
        
        private MoveResult getBestCounter(Move currentMove, Board b, bool isWhite)
        {
            double lowestScore = int.MaxValue;
            Board lowestCounterBoard = null;
            Board copy = b.DeepCopy();
            copy.makeMove(currentMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            foreach (Move counterMove in allCounters)
            {
                Board endOfCounterBoard = copy.DeepCopy();
                endOfCounterBoard.makeMove(counterMove);
                double currentScore = getScore(endOfCounterBoard, isWhite);
                if (currentScore < lowestScore)
                {
                    lowestScore = currentScore;
                    lowestCounterBoard = endOfCounterBoard;
                }
            }
            return new MoveResult(currentMove, lowestScore, lowestCounterBoard);
        }
        private double getScore(Board b, bool isWhite)
        {
            double r = 0;

            foreach(IPieces p in b.allPeices)
            {
                char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
                int value = peiceValues[type];
                r = (p.isWhite == isWhite) ? r + value : r - value;
            }

            // number of possible moves
            List<Move> ours = b.getAllMoves(isWhite);
            List<Move> opp = b.getAllMoves(!isWhite);
            int moveNumberDiff = ours.Count - opp.Count;
            r += moveNumberDiff * 0.1;

            //doubled pawn
            for(int x = 0; x <= 7; x++)
            {
                int whitePawns = 0;
                int blackPawns = 0;
                for(int y = 0; y <= 7; y++)
                {
                    if (b.layout[x, y] != null)
                    {
                        if (b.layout[x, y].ToUpper()[1] == 'P')
                        {
                            if (b.layout[x, y].ToUpper()[0] == 'W')
                            {
                                whitePawns++;
                            }
                            else
                            {
                                blackPawns++;
                            }
                        }
                    }
                }
                if (whitePawns > 1)
                {
                    r += 0.5 * (isWhite ? -1 : 1);
                }
                if (blackPawns > 1)
                {
                    r += 0.5 * (isWhite ? 1 : -1);
                }
            }


            
            // protected pieces value
            // threatened peices
            // threatened uprotected pieces
            // if game is nearly over, number of king moves avialable

            //-0.5(D - D' + S-S' + I - I')
            //D, S, I = doubled, blocked and isolated pawns
            return r;
        }

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
            MoveResult bestFirstCounter = getBestCounter(firstMove, b, isWhite);

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
                bestCounters.Add(getBestCounter(move, b.DeepCopy(), isWhite));
            }

            if (depthLeft == 0)
            {
                return prune(bestCounters, true)[0];
            }
            else
            {
                List<MoveResult> prunedResults = prune(bestCounters);

                //recursion

                List<MoveResult> moveResultsSecond = new List<MoveResult>();
                foreach (MoveResult move in prunedResults)
                {
                    moveResultsSecond.Add(getMoveIteration(move.boardAfterMove, isWhite, depthLeft -1));
                }

                return prune(moveResultsSecond, true)[0];
            }
        }
        private List<MoveResult> prune(List<MoveResult> all, bool getHighest = false)
        {
            List<MoveResult> ordered = all.OrderByDescending(o => o.score).ToList();

            if(ordered.Count > this.maxWidth)
            {
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
                        else
                        {
                            break;
                        }
                    }
                    if (toRGT.Count > this.maxWidth)
                    {
                        return toRGT.Take(this.maxWidth).ToList();
                    }
                    else if (toRGT.Count + toREQ.Count > this.maxWidth)
                    {
                        List<MoveResult> toR = toRGT;
                        Random random = new Random();
                        toR.AddRange(toREQ.OrderBy(x => random.Next()).Take(toR.Count - this.maxWidth).ToList());
                        return toR;
                    }
                    else
                    {
                        List<MoveResult> toR = toRGT;
                        toR.AddRange(toREQ);
                        return toR;
                    }                    
                }
            }
            else
            {
                return ordered;
            }
        }
    }
}
