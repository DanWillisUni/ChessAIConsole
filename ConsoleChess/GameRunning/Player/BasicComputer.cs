using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class BasicComputer : BasicPlayer, IPlayer
    {
        public bool isWhite { get; set; }
        public BasicComputer(bool isWhite)
        {
            this.isWhite = isWhite;
        }

        public Move makeTurn(Board b)
        {
            List<Move> all = getAllMoves(isWhite,b);
            Random random = new Random();
            return all[random.Next(all.Count)];
        }
    }
}
