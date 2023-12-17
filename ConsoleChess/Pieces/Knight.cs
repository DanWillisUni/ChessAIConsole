using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    [Serializable]
    public class Knight : BasicPiece, IPieces
    {
        public Knight(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesKnight(this, b);
        }
        public IPieces DeepCopy()
        {
            Knight deepcopy = new Knight(this.id, this.numberOfMoves, this.location.DeepCopy());
            return deepcopy;
        }
    }
}
