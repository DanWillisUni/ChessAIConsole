using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI.Variations
{
    public interface IComputer
    {
        public Move getMove(Board b, bool isWhite);
    }
}
