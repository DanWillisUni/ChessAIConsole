using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public interface IPlayer
    {
        public bool isWhite { get; set; }
        public Move makeTurn();
    }
}
