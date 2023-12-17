using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static ConsoleChess.AI.ExtensionMethods;

namespace ConsoleChess.Pieces
{
    public interface IPieces
    {        
        public string id { get; set; }
        public bool isWhite { get; set; }
        public int numberOfMoves { get; set; }
        public Location location { get; set; }

        IPieces DeepCopy();
        public List<Move> getPossibleMoves(Board b);

    }
}
