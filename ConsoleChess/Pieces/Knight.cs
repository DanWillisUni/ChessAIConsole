using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    public class Knight : BasicPiece,IPieces
    {
        public Knight(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getAllMoves(Board b)
        {
            return getPossibleMovesKnight(this, b);
        }
    }
}
