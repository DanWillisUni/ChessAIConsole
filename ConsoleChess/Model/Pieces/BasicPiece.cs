using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.Pieces
{
    public class BasicPiece : IPieces
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
        public BasicPiece(Location location) 
        {
            this.id = "empty";
            this.location = location;
        }

        public List<Move> getPossibleMovesPawn(IPieces pieceToMove,Board board)
        {
            throw new NotImplementedException();
        }
        public List<Move> getPossibleMovesRook(IPieces pieceToMove, Board board)
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
        public List<Move> getPossibleMovesKnight(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
        }
        public List<Move> getPossibleMovesBishop(IPieces pieceToMove, Board board)
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
        public List<Move> getPossibleMovesQueen(IPieces pieceToMove, Board board)
        {
            List<Move> all = getPossibleMovesBishop(pieceToMove, board);
            foreach(Move m in getPossibleMovesBishop(pieceToMove, board))
            {
                all.Add(m);
            }
            return all;
        }
        public List<Move> getPossibleMovesKing(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
        }

        private List<Move> getMovesInDirection(IPieces pieceToMove,int x,int y,Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            bool canContinue = true;
            int count = 0;
            do
            {
                count++;
                List<IPieces> xLine = board.boardLayout[fromXCoord + (x * count)];
                IPieces current = xLine[fromYCoord + (y * count)];
                if (current.id == "empty")
                {
                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                }
                else if (current.isWhite != pieceToMove.isWhite)
                {
                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                    canContinue = false;
                }
                else
                {
                    canContinue = false;
                }
            } while (canContinue);
            return moves;
        }

        public List<Move> getAllMoves()
        {
            throw new NotImplementedException();
        }




    }
}
