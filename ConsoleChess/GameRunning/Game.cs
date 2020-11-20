using ConsoleChess.GameRunning.Player;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning
{
    public class Game
    {
        Board board { get; set; }
        public bool isWhitesTurn { get; set; }
        public IPlayer white { get; set; }
        public IPlayer black { get; set; }
        public List<Move> pastMoves { get; set; }

        public Game(IPlayer white,IPlayer black)
        {
            board = new Board();
            isWhitesTurn = true;
            this.white = white;
            this.black = black;
            pastMoves = new List<Move>();
        }
    }
}
