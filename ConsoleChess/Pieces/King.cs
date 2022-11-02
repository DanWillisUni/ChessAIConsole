using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    [Serializable]
    public class King : BasicPiece,IPieces
    {
        public bool hasCastled { get; set; } 
        public King(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        {
            hasCastled = false;
        }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesKing(this,b);

        }
    }
}
