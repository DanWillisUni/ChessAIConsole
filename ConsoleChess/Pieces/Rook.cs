using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    public class Rook : BasicPiece,IPieces
    {
        public Rook(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getAllMoves(Board b)
        {
            return getPossibleMovesRook(this, b);
        }
    }
}
