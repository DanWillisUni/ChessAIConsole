using ConsoleChess.AI;
using ConsoleChess.AI.Openings;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class BasicComputer : BasicPlayer, IPlayer
    {
        public bool isWhite { get; set; }
        private OpeningFileStructure openings {  get; set; }
        public BasicComputer(bool isWhite, OpeningFileStructure openings)
        {
            this.isWhite = isWhite;
            this.openings = openings;
        }

        public Move makeTurn(Board b)
        {
            ComputerBase c = new ComputerBase(openings);
            return c.getMove(b,isWhite);
        }
    }
}
