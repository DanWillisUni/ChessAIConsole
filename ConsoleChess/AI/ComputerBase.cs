using ConsoleChess.AI.Model;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI
{
    public class ComputerBase
    {
        private readonly object moveLock = new object();//mutex object for multi threading
        private readonly Dictionary<Char, Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 100 }, { 'B', 300 }, { 'N', 300 }, { 'R', 500 }, { 'Q', 900 }, { 'K', 10000 } };
        public Move getMove(Board b,bool isWhite)
        {
            b.print(!isWhite);
            List<Move> all = b.getAllMoves(isWhite);
            if (b.pastMoves.Count < 6)
            {
                Console.WriteLine("Searching openings...");
                //search openings
                //return if in openings db
            }

            Console.WriteLine("Calculating...");

            List<MoveResult> results = new List<MoveResult>();
            Parallel.For<List<MoveResult>>(0, all.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
            {
                threadResults.Add(evaluateMove(all[(int)i], b, isWhite, 1));
                return threadResults;//return the thread results
            },
            (threadResults) => {
                lock (moveLock)//lock the list
                {
                    results.AddRange(threadResults);//add the thread results to the results list
                }
            });

            List<Move> r = new List<Move>();
            int currentHighest = int.MinValue;
            foreach (MoveResult mr in results)
            {
                if (mr.score >= currentHighest)
                {
                    if (currentHighest > mr.score)
                    {
                        currentHighest = mr.score;
                        r = new List<Move>();
                    }
                    r.Add(mr.m);
                }
            }
            Random random = new Random();
            return r[random.Next(r.Count)];
        }  
        
        private MoveResult evaluateMove(Move firstMove,Board b,bool isWhite,int maxDepth)
        {
            int lowestScore = int.MaxValue;
            Board copy = b.DeepClone();
            copy.makeMove(firstMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            foreach (Move m1 in allCounters)
            {
                Board copy1 = b.DeepClone();
                copy1.makeMove(m1);
                if(maxDepth == 0)
                {
                    int currentScore = getScore(copy1, isWhite);
                    if (currentScore < lowestScore)
                    {
                        lowestScore = currentScore;
                    }
                }
                else
                {
                    lowestScore = getLowestScore(copy1, isWhite, 1, maxDepth);
                }
            }
            return new MoveResult(firstMove, lowestScore);
        }
        private int getLowestScore(Board b, bool isWhite,int currentDepth, int maxDepth)
        {
            int highestLowScore = int.MinValue;
            List<Move> all = b.getAllMoves(isWhite);
            foreach (Move m in all)
            {
                int lowestScore = int.MaxValue;
                Board copy = b.DeepClone();
                copy.makeMove(m);
                List<Move> allCounters = copy.getAllMoves(!isWhite);
                foreach (Move m1 in allCounters)
                {
                    Board copy1 = b.DeepClone();
                    copy1.makeMove(m1);
                    if (maxDepth == currentDepth)
                    {
                        int currentScore = getScore(copy1, isWhite);
                        if (currentScore < lowestScore)
                        {
                            lowestScore = currentScore;
                        }
                    }
                    else
                    {
                        lowestScore = getLowestScore(copy1, isWhite, currentDepth + 1, maxDepth);
                    }
                }
                if(highestLowScore < lowestScore)
                {
                    highestLowScore = lowestScore;
                }
            }
            return highestLowScore;
        }
        private int getScore(Board b,bool isWhite)
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
    }
}
