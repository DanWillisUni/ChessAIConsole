using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class BasicComputer : IPlayer
    {
        public bool isWhite { get; set; }
        public BasicComputer(bool isWhite)
        {
            this.isWhite = isWhite;
        }

        public Move makeTurn()
        {
            throw new NotImplementedException();
        }
    }
}
