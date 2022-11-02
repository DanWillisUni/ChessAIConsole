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
        public Move getMove(Board b,bool isWhite)
        {
            List<Move> all = b.getAllMoves(isWhite);
            if (b.pastMoves.Count < 6)
            {
                //search openings
                //return if in openings db
            }

            List<MoveResult> results = new List<MoveResult>();
            Parallel.For<List<MoveResult>>(0, all.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
            {
                threadResults.AddRange(getMovesToPlay(all[(int)i], b, isWhite, 1));
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
        
        private List<MoveResult> getMovesToPlay(Move firstMove,Board b,bool isWhite,int maxDepth)
        {
            List<MoveResult> r = new List<MoveResult>();
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
            r.Add(new MoveResult(firstMove, lowestScore));
            return r;
        }
        private int getLowestScore(Board b, bool isWhite,int currentDepth, int maxDepth)
        {
            List<Move> all = b.getAllMoves(isWhite);
            int lowestScore = int.MaxValue;
            foreach (Move m in all)
            {    
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
            }
            return lowestScore;
        }

        private static int getScore(Board b,bool isWhite)
        {
            int r = 0;
            Dictionary<Char,Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 100 },{ 'B',300},{ 'N',300},{ 'R',500},{ 'Q',900},{ 'K',10000} }; 

            foreach(IPieces p in b.allPeices)
            {
                char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
                int value = peiceValues[type];
                r = (p.isWhite == isWhite) ? r + value : r - value;
            }
            return r;
        }
    }
}
