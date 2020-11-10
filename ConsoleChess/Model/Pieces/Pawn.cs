using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.Pieces
{
    public class Pawn : BasicPiece
    {
        char moveType { get; set; }
        
        public Pawn(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        {
            moveType = 'P';
        }

        public List<Move> getAllMoves(Board b)
        {
            return getPossibleMovesPawn(this, b);
        }
    }
}
