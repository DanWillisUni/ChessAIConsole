using ConsoleChess.AI;
using ConsoleChess.AI.Openings;
using ConsoleChess.AI.Variations;
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
        private IComputer computer { get; set; }
        public BasicComputer(bool isWhite, OpeningFileStructure openings, string typeStr = "Blitz")
        {
            Factory f = new Factory(isWhite);
            this.isWhite = isWhite;
            computer = f.generateComputer(typeStr, openings);
        }

        public Move makeTurn(Board b)
        {
            return computer.getMove(b, isWhite);
        }
    }
}
