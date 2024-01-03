using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConsoleChesss
{
    [Serializable]
    public class BasicPiece
    {
        public string id { get; set; }
        public bool isWhite { get; set; }
        public int numberOfMoves { get; set; }
        public Location location { get; set; }
        public BasicPiece(string id, int numberOfMoves, Location location)
        {
            this.id = id;
            isWhite = id[0] == 'W' ? true : false;
            this.numberOfMoves = numberOfMoves;
            this.location = location;
        }
        #region peice movement
        public static List<Move> removeInCheck(List<Move> moves, bool isWhite, Board b)
        {
            List<Move> r = new List<Move>();
            foreach (Move m in moves)
            {
                Board copy = b.DeepCopy();
                copy.makeMove(m);
                if (!copy.isInCheck(isWhite))
                {
                    r.Add(m);
                }
            }
            return r;
        }
        public static List<Move> getPossibleMovesPawn(Pawn pieceToMove, Board board)
        {
            //Handle Promotions
            if (pieceToMove.moveType == 'Q')
            {
                return getPossibleMovesQueen(pieceToMove,board);
            }
            else if (pieceToMove.moveType == 'B')
            {
                return getPossibleMovesBishop(pieceToMove, board);
            }
            else if (pieceToMove.moveType == 'R')
            {
                return getPossibleMovesRook(pieceToMove, board);
            }
            else if (pieceToMove.moveType == 'N')
            {
                return getPossibleMovesKnight(pieceToMove, board);
            }

            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            int forwardMultiplyer = (pieceToMove.isWhite ? 1:-1);

            if (Board.isOnBoard(fromXCoord, fromYCoord + forwardMultiplyer))
            {
                if (String.IsNullOrEmpty(board.layout[fromXCoord, fromYCoord + forwardMultiplyer]))
                {
                    if (Board.isOnBoard(fromXCoord, fromYCoord + (2 * forwardMultiplyer)) && pieceToMove.numberOfMoves == 0)
                    {
                        if (String.IsNullOrEmpty(board.layout[fromXCoord, fromYCoord + (2 * forwardMultiplyer)]))
                        {
                            moves.Add(new Move(pieceToMove.location, new Location(fromXCoord, fromYCoord + (2 * forwardMultiplyer))));
                        }
                    }
                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord, fromYCoord + forwardMultiplyer)));
                }
            }

            //taking
            for (int x = -1; x < 2; x += 2)
            {
                if (Board.isOnBoard(fromXCoord + x, fromYCoord + forwardMultiplyer))
                {
                    if (!String.IsNullOrEmpty(board.layout[fromXCoord + x, fromYCoord + forwardMultiplyer]))
                    {
                        char takeColour = (board.layout[fromXCoord + x, fromYCoord + forwardMultiplyer])[0];
                        if (pieceToMove.isWhite ? takeColour == 'B' : takeColour == 'W')
                        {
                            moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + forwardMultiplyer)));
                        }
                    }
                }
            }

            //still need enpassant

            //return removeInCheck(moves, pieceToMove.isWhite, board);
            return moves;
        }
        public static List<Move> getPossibleMovesRook(IPieces pieceToMove, Board board)
        {
            List<Move> moves = getMovesInDirection(pieceToMove, 1, 0, board);
            foreach(Move newMoves in getMovesInDirection(pieceToMove, 0, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, 0, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, 0, 1, board))
            {
                moves.Add(newMoves);
            }
            //return removeInCheck(moves, pieceToMove.isWhite, board);
            return moves;
        }
        public static List<Move> getPossibleMovesKnight(IPieces pieceToMove, Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();

            for(int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    if(x != 0 && y!= 0 && Math.Pow(x,2) != Math.Pow(y, 2) && Board.isOnBoard(fromXCoord + x, fromYCoord + y))
                    {
                        string current = board.layout[fromXCoord + x, fromYCoord + y];
                        if (String.IsNullOrEmpty(current) || (pieceToMove.isWhite ? current[0] == 'B' : current[0] == 'W'))
                        {
                            moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + y)));
                        }
                    }
                }
            }

            //return removeInCheck(moves, pieceToMove.isWhite, board);
            return moves;
        }
        public static List<Move> getPossibleMovesBishop(IPieces pieceToMove, Board board)
        {
            List<Move> moves = getMovesInDirection(pieceToMove, 1, 1, board);
            foreach (Move newMoves in getMovesInDirection(pieceToMove, 1, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, 1, board))
            {
                moves.Add(newMoves);
            }
            //return removeInCheck(moves, pieceToMove.isWhite, board);
            return moves;
        }
        public static List<Move> getPossibleMovesQueen(IPieces pieceToMove, Board board)
        {
            List<Move> all = getPossibleMovesBishop(pieceToMove, board);
            all.AddRange(getPossibleMovesRook(pieceToMove, board));
            return all;
        }
        public static List<Move> getPossibleMovesKing(King pieceToMove, Board board, bool includeCastle = true)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();

            for(int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(!(x == 0 && y == 0))
                    {
                        if (Board.isOnBoard(fromXCoord + x, fromYCoord + y))
                        {
                            string current = board.layout[fromXCoord + x,fromYCoord + y];
                            if (String.IsNullOrEmpty(current))
                            {
                                moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + y)));
                            }
                            else
                            {
                                if (pieceToMove.isWhite ? current[0] == 'B' : current[0] == 'W')
                                {
                                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + y)));
                                }
                            }
                        }
                    }
                }
            }

            if (pieceToMove.numberOfMoves == 0 && includeCastle && !board.isInCheck(pieceToMove.isWhite))
            {
                //queenside
                if (String.IsNullOrEmpty(board.layout[fromXCoord - 1, fromYCoord]) && 
                    String.IsNullOrEmpty(board.layout[fromXCoord - 2, fromYCoord]) &&
                    String.IsNullOrEmpty(board.layout[fromXCoord - 3, fromYCoord]))
                {
                    IPieces rook = board.allPeices.Where(o => o.location.getYCoord() == fromYCoord && o.location.getXCoord() == fromXCoord - 4).Select(o => o).FirstOrDefault();
                    if (rook != null)
                    {
                        if (rook.id[0] == pieceToMove.id[0] &&
                            rook.id[1] == 'R' &&
                            rook.numberOfMoves == 0)
                        {
                            List<string> locationsToCheck = new List<string>();
                            locationsToCheck.Add(new Location(fromXCoord - 1, fromYCoord).ToString());
                            locationsToCheck.Add(new Location(fromXCoord - 2, fromYCoord).ToString());
                            locationsToCheck.Add(new Location(fromXCoord - 3, fromYCoord).ToString());

                            List<Move> oppMoves = board.getAllMoves(!pieceToMove.isWhite, false);
                            bool movingThroughCheck = false;
                            foreach (Move move in oppMoves)
                            {
                                if (locationsToCheck.Contains(move.toLocation.ToString()))
                                {
                                    movingThroughCheck = true; break;
                                }
                            }
                            if (!movingThroughCheck)
                            {
                                moves.Add(new Move(pieceToMove.location, new Location(fromXCoord - 2, fromYCoord)));
                            }
                        }
                    }
                }

                //kingside
                if (String.IsNullOrEmpty(board.layout[fromXCoord + 1, fromYCoord]) &&
                    String.IsNullOrEmpty(board.layout[fromXCoord + 2, fromYCoord]))
                {
                    IPieces rook = board.allPeices.Where(o => o.location.getYCoord() == fromYCoord && o.location.getXCoord() == fromXCoord + 3).Select(o => o).FirstOrDefault();
                    if (rook != null)
                    {
                        if (rook.id[0] == pieceToMove.id[0] &&
                            rook.id[1] == 'R' &&
                            rook.numberOfMoves == 0)
                        {
                            List<string> locationsToCheck = new List<string>();
                            locationsToCheck.Add(new Location(fromXCoord + 1, fromYCoord).ToString());
                            locationsToCheck.Add(new Location(fromXCoord + 2, fromYCoord).ToString());

                            List<Move> oppMoves = board.getAllMoves(!pieceToMove.isWhite, false);
                            bool movingThroughCheck = false;
                            foreach (Move move in oppMoves)
                            {
                                if (locationsToCheck.Contains(move.toLocation.ToString()))
                                {
                                    movingThroughCheck = true; break;
                                }
                            }
                            if (!movingThroughCheck)
                            {
                                moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + 2, fromYCoord)));
                            }
                        }
                    }
                }
            }

            return removeInCheck(moves, pieceToMove.isWhite, board);

        }
        private static List<Move> getMovesInDirection(IPieces pieceToMove, int x, int y, Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            bool canContinue = true;
            int count = 0;
            do
            {
                count++;
                if (!Board.isOnBoard(fromXCoord + (x * count), fromYCoord + (y * count)))
                {
                    canContinue = false;
                } 
                else
                {
                    string current = board.layout[fromXCoord + (x * count),fromYCoord + (y * count)];
                    if (String.IsNullOrEmpty(current))
                    {
                        moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                    }
                    else
                    { 
                        if (pieceToMove.isWhite ? current[0] == 'B' : current[0] == 'W')
                        {
                            moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                        }
                        canContinue = false;
                    }
                }
                
            } while (canContinue);
            return moves;
        }
        #endregion

    }
}
