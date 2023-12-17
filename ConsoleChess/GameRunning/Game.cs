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

        public Game(IPlayer white,IPlayer black)
        {
            board = new Board();
            isWhitesTurn = true;
            this.white = white;
            this.black = black;
        }

        public void Start()
        {
            while(true)
            {
                Move m = null;
                if (isWhitesTurn)
                {
                    m = white.makeTurn(board);
                    isWhitesTurn = false;
                } else {
                    m = black.makeTurn(board);
                    isWhitesTurn = true;
                }
                board.makeMove(m);
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
