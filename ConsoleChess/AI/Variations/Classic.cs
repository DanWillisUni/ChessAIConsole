using ConsoleChess.AI.Model;
using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI.Variations
{
    public class Classic : IComputer
    {
        public Classic(OpeningFileStructure openings, int maxDepth = 4, int maxWidth = 8)
        {
            this.openings = openings;
            this.maxDepth = maxDepth;
            this.maxWidth = maxWidth;
        }
        public int maxDepth { get; set; }
        public int maxWidth { get; set; }
        public OpeningFileStructure openings { get; set; }

        private readonly object moveLock = new object();//mutex object for multi threading

        public Move getMove(Board b, bool isWhite)
        {
            b.print(!isWhite);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Move move = null;
            if (b.pastMoves.Count <= 6)
            {
                move = ComputerBase.searchOpenings(openings, b, isWhite);
            }
            if (move == null)
            {
                move = calculateMove(b, isWhite);
            }
            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            string elapsedTimeStr = elapsedTime.ToString() + "ms";
            if (elapsedTime > 1000) { elapsedTimeStr = Math.Round((double)elapsedTime / 1000, 2).ToString() + "s"; }
            if (elapsedTime > 60000) { elapsedTimeStr = ((int)(elapsedTime / 60000)).ToString() + "m " + Math.Round((double)((elapsedTime % 60000) / 1000), 2).ToString() + "s"; }
            Console.WriteLine("Got move in: " + elapsedTimeStr);
            return move;
        }

        private MoveResult getBestCounter(Move currentMove, Board b, bool isWhite, bool printDebug = false)
        {
            double lowestScore = int.MaxValue;
            Board lowestCounterBoard = null;
            Board copy = b.DeepCopy();
            copy.makeMove(currentMove);
            List<Move> allCounters = copy.getAllMoves(!isWhite);
            if (printDebug) { Console.WriteLine("All counters:"); }
            foreach (Move counterMove in allCounters)
            {
                if (printDebug) { Console.WriteLine(counterMove.ToString()); }
                Board endOfCounterBoard = copy.DeepCopy();
                endOfCounterBoard.makeMove(counterMove);
                if (printDebug) { Console.WriteLine("Board: " + endOfCounterBoard.printAsString()); }
                double currentScore = getScore(endOfCounterBoard, isWhite);
                if (currentScore < lowestScore)
                {
                    lowestScore = currentScore;
                    lowestCounterBoard = endOfCounterBoard;
                }
            }
            return new MoveResult(currentMove, lowestScore, lowestCounterBoard);
        }

        #region scoring
        private double getScore(Board b, bool isWhite)
        {
            double kingSquareNotThreatenedCoeffiecient = 0.1;
            double peiceSquaredValueCoefficient = 1;
            double mobilityCoeffiecient = 0.1;
            double valueOfThreateningCoefficient = 0.1;
            double doublePawnCoefficient = 0.5;
            double blockedPawnCoefficient = 0.5;
            double pawnShieldCoefficient = 2;
            double castlingCoefficient = 2;
            double previousMovePunishment = 1;
            double turnCoefficient = 0.5;
            if (ComputerBase.isEndGame(b))
            {
                kingSquareNotThreatenedCoeffiecient = 0.3;
                castlingCoefficient = 3;
            }

            double r = 0;
            int forwardMultiplyer = isWhite ? 1 : -1;
            char isWhiteChar = isWhite ? 'W' : 'B';

            if (b.layout[b.pastMoves[b.pastMoves.Count - 1].toLocation.getXCoord(), b.pastMoves[b.pastMoves.Count - 1].toLocation.getYCoord()][0] == isWhiteChar) //if last move was scorer
            {
                r -= turnCoefficient;
                if (b.pastMoves.Count >= 3)
                {
                    if (b.pastMoves[b.pastMoves.Count - 3].fromLocation.Equals(b.pastMoves[b.pastMoves.Count - 1].toLocation))
                    {
                        r -= previousMovePunishment;
                    }
                }
            }
            else
            {
                r += turnCoefficient;
            }

            foreach (IPieces p in b.allPeices)
            {
                double value = Scoring.getPeiceValueStandard(p);
                r = (p.isWhite == isWhite) ? r + value : r - value;
                double peiceSquareValue = peiceSquaredValueCoefficient * Scoring.getPeiceSquareValue(p, ComputerBase.isEndGame(b));
                r = (p.isWhite == isWhite) ? r + peiceSquareValue : r - peiceSquareValue;
                if (p.id.ToUpper()[1] == 'K')
                {
                    King k = (King)p;
                    if (k.hasCastled)
                    {
                        r += isWhiteChar == k.id.ToUpper()[0] ? castlingCoefficient : castlingCoefficient * -1;
                    }
                }
            }

            // number of possible moves
            List<Move> ourMoves = b.getAllMoves(isWhite);
            List<Move> oppMoves = b.getAllMoves(!isWhite);
            int moveNumberDiff = ourMoves.Count - oppMoves.Count;
            r += moveNumberDiff * mobilityCoeffiecient;

            r += valueOfThreateningCoefficient * Scoring.getValueOfThreatening(b, ourMoves);
            r -= valueOfThreateningCoefficient * Scoring.getValueOfThreatening(b, oppMoves);

            r += kingSquareNotThreatenedCoeffiecient * Scoring.squaresAroundKingNotThreatened(b, isWhite);
            r -= kingSquareNotThreatenedCoeffiecient * Scoring.squaresAroundKingNotThreatened(b, !isWhite);

            for (int x = 0; x <= 7; x++)
            {
                int ourPawnsPerColumn = 0;
                int opponentsPawnsPerColumn = 0;
                for (int y = 0; y <= 7; y++)
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
                                        r -= blockedPawnCoefficient;
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
                                        r += blockedPawnCoefficient;
                                    }
                                }
                            }
                        }
                        else if (b.layout[x, y].ToUpper()[1] == 'K')
                        {
                            char kingColour = b.layout[x, y].ToUpper()[0];
                            int peiceForwardMultiplyer = isWhiteChar == kingColour ? forwardMultiplyer : forwardMultiplyer * -1;
                            bool hasPawnShield = true;
                            for (int xIncrease = -1; xIncrease <= 1; xIncrease++)
                            {
                                if (hasPawnShield)
                                {
                                    if (Board.isOnBoard(x + xIncrease, y + (peiceForwardMultiplyer * 1)))
                                    {
                                        string currentID = "";
                                        if (b.layout[x + xIncrease, y + (peiceForwardMultiplyer * 1)] != null)
                                        {
                                            currentID = b.layout[x + xIncrease, y + (peiceForwardMultiplyer * 1)].ToUpper();
                                        }
                                        else if (b.layout[x + xIncrease, y + (peiceForwardMultiplyer * 2)] != null)
                                        {
                                            currentID = b.layout[x + xIncrease, y + (peiceForwardMultiplyer * 2)].ToUpper();
                                        }
                                        else
                                        {
                                            hasPawnShield = false;
                                            break;
                                        }
                                        if (currentID[1] == 'P' && currentID[0] == kingColour)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            hasPawnShield = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (hasPawnShield)
                            {
                                r += (kingColour == isWhiteChar ? pawnShieldCoefficient : -1 * pawnShieldCoefficient);
                            }
                        }
                    }
                }
                if (ourPawnsPerColumn > 1)
                {
                    r -= doublePawnCoefficient;
                }
                if (opponentsPawnsPerColumn > 1)
                {
                    r += doublePawnCoefficient;
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
        #endregion
        private Move calculateMove(Board b, bool isWhite)
        {
            Console.WriteLine("Calculating...");

            List<Move> all = b.getAllMoves(isWhite);
            if (all.Count == 1)
            {
                return all[0];
            }

            List<Move> movesToSearch = new List<Move>();
            List<MoveResult> movesResultsToSearch = new List<MoveResult>();
            if (maxDepth >= 2) //if the depth is 2 or more
            {
                List<MoveResult> firstResult = new List<MoveResult>();
                Parallel.For<List<MoveResult>>(0, all.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
                {
                    threadResults.Add(getBestCounter(all[(int)i], b, isWhite));
                    return threadResults;//return the thread results
                },
                (threadResults) => {
                    lock (moveLock)//lock the list
                    {
                        firstResult.AddRange(threadResults);//add the thread results to the results list
                    }
                });

                List<MoveResult> prunedFirstResult = prune(firstResult, false, true);//prune

                if (prunedFirstResult.Count > 1)//if the prune is more than one add the moves
                {
                    foreach (MoveResult mr in prunedFirstResult)
                    {
                        movesToSearch.Add(mr.originalMove);
                    }
                }
                else // if the prune is one or less add the move result
                {
                    foreach (MoveResult mr in prunedFirstResult)
                    {
                        movesResultsToSearch.Add(mr);
                    }
                }
            }
            else // if the depth is less than 2, search all
            {
                movesToSearch = all;
            }

            List<MoveResult> results = new List<MoveResult>();
            List<MoveResult> r = new List<MoveResult>();
            if (movesToSearch.Count > 1) //if the moves to search is greater than 1
            {
                Parallel.For<List<MoveResult>>(0, movesToSearch.Count, () => new List<MoveResult>(), (i, loop, threadResults) => //multithreaded for loop
                {
                    threadResults.Add(getMoveFirstIteration(movesToSearch[(int)i], b, isWhite));
                    return threadResults;//return the thread results
                },
                (threadResults) =>
                {
                    lock (moveLock)//lock the list
                    {
                        results.AddRange(threadResults);//add the thread results to the results list
                    }
                });

                foreach (MoveResult result in results)
                {
                    Console.WriteLine(result.ToString());
                }

                r = prune(results, true);
            }
            else if (movesToSearch.Count == 1)
            {
                return movesToSearch[0];
            }
            else
            {
                r = movesResultsToSearch;
            }

            Console.WriteLine("Highest results");
            foreach (MoveResult highestFinalResults in r)
            {
                Console.WriteLine(highestFinalResults.ToString());
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
        private List<MoveResult> prune(List<MoveResult> all, bool getHighest = false, bool debug = false)
        {
            if (all.Count > this.maxWidth && this.maxWidth > 0)
            {
                List<double> scores = new List<double>();
                foreach(MoveResult mr in all)
                {
                    scores.Add(mr.score);
                }
                List<double> orderedScores = scores.OrderByDescending(o => o).ToList();
                double scoreToBeat = orderedScores[0];
                if (!getHighest)
                {
                    double maxWidthScore = orderedScores[this.maxWidth];
                    double deivationScore = Math.Round(orderedScores[0] - Math.Round(0.2 * Math.Abs(orderedScores[0]), 2), 2);
                    scoreToBeat = Math.Max(maxWidthScore, deivationScore);
                    if(debug)
                    {
                        Console.WriteLine($"Hi: {orderedScores[0]}, maxWidth: {maxWidthScore}, div: {deivationScore}, stb: {scoreToBeat}");
                    }
                }
                using (IEnumerator<MoveResult> resultsEnumerator = all.GetEnumerator())
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
