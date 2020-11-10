﻿using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.Pieces
{
    public class Rook : BasicPiece
    {
        public Rook(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        { }
    }
}
