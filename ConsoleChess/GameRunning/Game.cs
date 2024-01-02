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
        public IPlayer whitePlayer { get; set; }
        public IPlayer blackPlayer { get; set; }

        public Game(IPlayer whitePlayer, IPlayer blackPlayer)
        {
            board = new Board();
            isWhitesTurn = true;
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
        }

        public string run()
        {
            bool isCheckmate = false;
            bool isStalemate = false;
            while(!isCheckmate && !isStalemate)
            {
                MoveReturn m = isWhitesTurn ? whitePlayer.makeTurn(board) : blackPlayer.makeTurn(board);
                if (string.IsNullOrEmpty(m.command))
                {
                    isWhitesTurn = !isWhitesTurn;
                    board.makeMove(m.move);
                    if (board.isInCheck(isWhitesTurn))
                    {
                        if (board.isCheckmate(isWhitesTurn))
                        {
                            Console.WriteLine("Checkmate");
                            isCheckmate = true;
                        }
                        else
                        {
                            Console.WriteLine("Check");
                        }
                    }
                    if (!isCheckmate)
                    {
                        if (board.isStalemate(isWhitesTurn))
                        {
                            Console.WriteLine("Stalemate");
                            isStalemate = true;
                        }
                    }
                }
                else
                {
                    return m.command;
                }
            }
            if (isCheckmate)
            {
                string winner = isWhitesTurn ? "Black" : "White";
                Console.WriteLine(winner + " wins!");
                return "CHECKMATE";
            }
            else if (isStalemate)
            {
                return "STALEMATE";
            }
            return "END";
        }

        public void save(string filePath)
        {
            
        }
        public static Game load(string filePath)
        {
            Game game = null;
            return game;
        }
    }
}
