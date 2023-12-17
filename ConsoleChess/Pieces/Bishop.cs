using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    [Serializable]
    public class Bishop : BasicPiece, IPieces
    {
        public Bishop(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesBishop(this, b);
        }
    }
}
