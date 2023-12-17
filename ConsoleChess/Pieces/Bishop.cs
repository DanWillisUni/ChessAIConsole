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

        public IPieces DeepCopy()
        {
            Bishop deepcopy = new Bishop(this.id, this.numberOfMoves, this.location.DeepCopy());
            return deepcopy;
        }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesBishop(this, b);
        }
    }
}
