using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.Pieces
{
    public class Bishop : BasicPiece
    {
        public Bishop(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }
    }
}
