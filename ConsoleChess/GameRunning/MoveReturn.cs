using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.GameRunning
{
    public class MoveReturn
    {
        public string command {  get; set; }
        public Move move { get; set; }
        public MoveReturn(string command, Move move) 
        {
            this.command = command;
            this.move = move;
        }
    }
}
