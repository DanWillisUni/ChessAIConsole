using ConsoleChess.AI;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class BasicComputer : BasicPlayer, IPlayer
    {
        public bool isWhite { get; set; }
        private string openingFile {  get; set; }
        public BasicComputer(bool isWhite, string openingFile)
        {
            this.isWhite = isWhite;
            this.openingFile = openingFile;
        }

        public Move makeTurn(Board b)
        {
            ComputerBase c = new ComputerBase(openingFile);
            return c.getMove(b,isWhite);
        }
    }
}
