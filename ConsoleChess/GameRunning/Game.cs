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

        public void Start()
        {
            for(int i = 0; i < 3; i++)
            {

                if (isWhitesTurn)
                {
                    white.makeTurn();
                    isWhitesTurn = false;
                } else {
                    black.makeTurn();
                    isWhitesTurn = true;
                }
            }            
        }

        private bool isStalemate()
        {
            throw new NotImplementedException();
        }
        private bool isCheckmate()
        {
            throw new NotImplementedException();
        }
    }
}
