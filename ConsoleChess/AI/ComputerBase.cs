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
            if (b.pastMoves.Count < 8)
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
        
        private MoveResult evaluateCounters(Move currentMove, Board b, bool isWhite, Move originalMove = null)
        {
            int lowestScore = int.MaxValue;
            Board copy = b.DeepClone();
            copy.makeMove(currentMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            foreach (Move counterMove in allCounters)
            {
                Board endOfCounterBoard = b.DeepClone();
                endOfCounterBoard.makeMove(counterMove);
                int currentScore = getScore(endOfCounterBoard, isWhite);
                if (currentScore < lowestScore)
                {
                    lowestScore = currentScore;
                }
            }
            return new MoveResult(originalMove, currentMove, lowestScore, copy);
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
            List<MoveResult> results = getMovesSingleIteration(null, b, isWhite, this.maxDepth);

            Dictionary<Move,int> lowestScoresPerOriginalMove = new Dictionary<Move,int>();
            foreach(MoveResult moveResult in results)
            {
                if (lowestScoresPerOriginalMove.ContainsKey(moveResult.originalMove)) 
                {
                    if (lowestScoresPerOriginalMove[moveResult.originalMove] > moveResult.score)
                    {
                        lowestScoresPerOriginalMove[moveResult.originalMove] = moveResult.score;
                    }
                }
                else
                {
                    lowestScoresPerOriginalMove.Add(moveResult.originalMove, moveResult.score);
                }
            }

            List<Move> r = new List<Move>();
            int currentHighest = int.MinValue; 
            foreach (KeyValuePair<Move, int> entry in lowestScoresPerOriginalMove)
            {
                // Console.WriteLine(entry.Key.ToString() + " - " + entry.Value);
                if (entry.Value >= currentHighest)
                {
                    if (entry.Value > currentHighest)
                    {
                        currentHighest = entry.Value;
                        r = new List<Move>();
                    }
                    r.Add(entry.Key);
                }
            }
            //Console.WriteLine("Highest Score");
            Random random = new Random();
            return r[random.Next(r.Count)];
        }

        private List<MoveResult> getMovesSingleIteration(Move firstMove, Board b, bool isWhite, int depthLeft)
        {
            List<Move> all = b.getAllMoves(isWhite);

            List<MoveResult> results = new List<MoveResult>();
            Parallel.For<List<MoveResult>>(0, all.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
            {
                threadResults.Add(evaluateCounters(all[(int)i], b, isWhite, firstMove));
                return threadResults;//return the thread results
            },
            (threadResults) => {
                lock (moveLock)//lock the list
                {
                    results.AddRange(threadResults);//add the thread results to the results list
                }
            });

            if (depthLeft == 0)
            {
                return results;
            }
            else
            {
                List<MoveResult> prunedResults = prune(results);
                List<MoveResult> toR = new List<MoveResult>();
                foreach(MoveResult m in prunedResults)
                {
                    toR.AddRange(getMovesSingleIteration(m.originalMove, m.boardAfterMove, isWhite, depthLeft - 1));
                }
                return toR;
            }

        }
        private List<MoveResult> prune(List<MoveResult> all)
        {
            List<MoveResult> ordered = all.OrderByDescending(o => o.score).ToList();
            if(ordered.Count > this.maxWidth)
            {
                int scoreToBeat = ordered[this.maxWidth].score;
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
                    return toR;
                }
            }
            else
            {
                return ordered;
            }
        }
    }
}
