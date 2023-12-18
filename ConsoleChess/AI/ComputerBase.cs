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

namespace ConsoleChess.AI
{
    public class ComputerBase
    {
        public ComputerBase(OpeningFileStructure openings, int maxDepth = 2, int maxWidth=3)
        {
            this.openings = openings;
            this.maxDepth = maxDepth;
            this.maxWidth = maxWidth;
        }
        public int maxDepth { get; set; }
        public int maxWidth { get; set; }
        public OpeningFileStructure openings {  get; set; }

        private readonly object moveLock = new object();//mutex object for multi threading
        private readonly Dictionary<Char, Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 100 }, { 'B', 300 }, { 'N', 300 }, { 'R', 500 }, { 'Q', 900 }, { 'K', 10000 } };
        public Move getMove(Board b,bool isWhite)
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
            int lowestScore = int.MaxValue;
            Move lowestCounter = null;
            Board copy = b.DeepCopy();
            copy.makeMove(currentMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            foreach (Move counterMove in allCounters)
            {
                Board endOfCounterBoard = b.DeepCopy();
                endOfCounterBoard.makeMove(counterMove);
                int currentScore = getScore(endOfCounterBoard, isWhite);
                if (currentScore < lowestScore)
                {
                    lowestScore = currentScore;
                    lowestCounter = counterMove;
                }
            }
            copy.makeMove(lowestCounter);
            return new MoveResult(currentMove, lowestScore, copy);
        }
        private int getScore(Board b, bool isWhite)
        {
            int r = 0;

            foreach(IPieces p in b.allPeices)
            {
                char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
                int value = peiceValues[type];
                r = (p.isWhite == isWhite) ? r + value : r - value;
            }

            // number of possible moves
            // protected pieces value
            // threatened peices
            // threatened uprotected pieces
            // if game is nearly over, number of king moves avialable
            return r;
        }

        private Move calculateMove(Board b, bool isWhite)
        {
            Console.WriteLine("Calculating...");

            List<Move> all = b.getAllMoves(isWhite);
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

            List<MoveResult> r = prune(results, true);
 
            Random random = new Random();
            return r[random.Next(r.Count)].originalMove;
        }/*

        private List<MoveResult> getMovesSingleIteration(Move firstMove, Board b, bool isWhite, int depthLeft)
        {
            






            if (depthLeft == 0)
            {
                return results;
            }
            else
            {
                List<MoveResult> prunedResults = prune(results);

                if (prunedResults.Count == 1)
                {
                    return prunedResults;
                }
                else
                {
                    List<MoveResult> toR = new List<MoveResult>();
                    foreach (MoveResult m in prunedResults)
                    {
                        toR.AddRange(getMovesSingleIteration(firstMove, m.boardAfterMove, isWhite, depthLeft - 1));
                    }
                    return toR;
                }
            }

        }*/

        private MoveResult getMoveFirstIteration(Move firstMove, Board b, bool isWhite)
        {
            MoveResult bestFirstCounter = getBestCounter(firstMove, b, isWhite);
            /*List<Move> secondMoves = bestFirstCounter.boardAfterMove.getAllMoves(isWhite);
            foreach (Move move in secondMoves)
            {
                MoveResult mr = getBestCounter(move, bestFirstCounter.boardAfterMove, isWhite);
            }*/

            return bestFirstCounter;
        }
        private List<MoveResult> prune(List<MoveResult> all, bool getHighest = false)
        {
            List<MoveResult> ordered = all.OrderByDescending(o => o.score).ToList();

            if(ordered.Count > this.maxWidth)
            {
                int maxWidthScore = ordered[this.maxWidth].score;
                int scoreToBeat = ordered[0].score;
                if (!getHighest)
                {
                    int deivationScore = (int)(ordered[0].score - Math.Round(0.1 * Math.Abs(ordered[0].score)));
                    scoreToBeat = Math.Max(maxWidthScore, deivationScore);
                }
                using (IEnumerator<MoveResult> resultsEnumerator = ordered.GetEnumerator())
                {
                    List<MoveResult> toR = new List<MoveResult>();
                    while (resultsEnumerator.MoveNext())
                    {
                        MoveResult result = resultsEnumerator.Current;
                        if (result.score >= scoreToBeat)
                        {
                            toR.Add(result);
                        }
                        else
                        {
                            break;
                        }
                    }
                    return toR.Take(this.maxWidth).ToList();
                }
            }
            else
            {
                return ordered;
            }
        }
    }
}
