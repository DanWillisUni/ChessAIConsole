using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
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
        public static List<Move> getPossibleMovesPawn(Pawn pieceToMove,Board board)
        {
            throw new NotImplementedException();
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
            return moves;
        }
        public static List<Move> getPossibleMovesKnight(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
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
            return moves;
        }
        public static List<Move> getPossibleMovesQueen(IPieces pieceToMove, Board board)
        {
            List<Move> all = getPossibleMovesBishop(pieceToMove, board);
            foreach(Move m in getPossibleMovesBishop(pieceToMove, board))
            {
                all.Add(m);
            }
            return all;
        }
        public static List<Move> getPossibleMovesKing(King pieceToMove, Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();

            for(int x = -1;x > 1; x++)
            {
                for (int y = -1; y > 1; y++)
                {
                    if(!(x == 0 && y == 0))
                    {
                        if (!(fromXCoord + x < 0 || fromXCoord + x > 7 || fromYCoord + y > 7 || fromYCoord + y < 0))
                        {
                            List<IPieces> xLine = board.boardLayout[fromXCoord + x];
                            IPieces current = xLine[fromYCoord + y];
                            if (current == null || current.isWhite != pieceToMove.isWhite)
                            {
                                if (!board.isInCheck(pieceToMove))
                                {
                                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + y)));
                                }
                            }                            
                        }
                    }
                }
            }
            return moves;
            
        }
        private static List<Move> getMovesInDirection(IPieces pieceToMove,int x,int y,Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            bool canContinue = true;
            int count = 0;
            do
            {
                count++;
                if (fromXCoord + (x * count) < 0|| fromXCoord + (x * count)>7|| fromYCoord + (y * count)>7|| fromYCoord + (y * count)<0)
                {
                    canContinue = false;
                } 
                else
                {
                    List<IPieces> xLine = board.boardLayout[fromXCoord + (x * count)];
                    IPieces current = xLine[fromYCoord + (y * count)];
                    if (current == null)
                    {
                        moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                    }
                    else
                    { 
                        if (current.isWhite != pieceToMove.isWhite)
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
