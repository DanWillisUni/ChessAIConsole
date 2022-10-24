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

        public Move makeTurn()
        {
            bool invalidMove = true;
            Move r = null;
            while (invalidMove)
            {
                Console.WriteLine("What peice would you like to move");
                string fromStr = Console.ReadLine();
                Console.WriteLine("Where would you like to move to");
                string toStr = Console.ReadLine();

                r = new Move(new Location(fromStr), new Location(toStr));
                if (getAllMoves().Contains(r))
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
