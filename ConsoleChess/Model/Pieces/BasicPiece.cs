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
        public BasicPiece() { }

        public List<Move> getPossibleMovesPawn(IPieces pieceToMove,Board board)
        {
            throw new NotImplementedException();
        }
        public List<Move> getPossibleMovesRook(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
        }
        public List<Move> getPossibleMovesKnight(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
        }
        public List<Move> getPossibleMovesBishop(IPieces pieceToMove, Board board)
        {
            throw new NotImplementedException();
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
        public List<Move> getAllMoves()
        {
            throw new NotImplementedException();
        }
    }
}
