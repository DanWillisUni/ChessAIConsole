using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    [Serializable]
    public class Rook : BasicPiece, IPieces
    {
        public Rook(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesRook(this, b);
        }

        public IPieces DeepCopy()
        {
            Rook deepcopy = new Rook(this.id, this.numberOfMoves, this.location.DeepCopy());
            return deepcopy;
        }
    }
}
