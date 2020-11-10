using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Services
{
    public class GameRunning
    {
        Board board { get; }

        public GameRunning()
        {
            board = new Board();
        }
    }
}
