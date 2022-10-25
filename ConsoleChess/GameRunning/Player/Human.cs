using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class Human : BasicPlayer, IPlayer
    {
        public bool isWhite { get; set; }

        public Human(bool isWhite)
        {
            this.isWhite = isWhite;
        }

        public Move makeTurn(Board b)
        {
            bool invalidMove = true;
            Move r = null;
            while (invalidMove)
            {
                b.print(isWhite);
                Console.WriteLine("What peice would you like to move");
                string fromStr = Console.ReadLine().ToUpper();
                Console.WriteLine("Where would you like to move to");
                string toStr = Console.ReadLine().ToUpper();

                r = new Move(new Location(fromStr), new Location(toStr));
                if (getAllMoves(isWhite,b).Contains(r))
                {
                    invalidMove = false;
                }
                else
                {
                    Console.WriteLine("Invalid move");
                    Console.ReadLine();
                }
            }            

            return r;

        }
    }
}
