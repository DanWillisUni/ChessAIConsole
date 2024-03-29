﻿using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    [Serializable]
    public class Queen : BasicPiece, IPieces
    {
        public Queen(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }

        public List<Move> getPossibleMoves(Board b)
        {
            return getPossibleMovesQueen(this, b);
        }
        public IPieces DeepCopy()
        {
            Queen deepcopy = new Queen(this.id, this.numberOfMoves, this.location.DeepCopy());
            return deepcopy;
        }
    }
}
