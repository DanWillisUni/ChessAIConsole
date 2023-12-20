using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

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
            bool invalidEntry = true;
            Move r = null;
            while (invalidEntry)
            {
                b.print(isWhite);
                Console.WriteLine("What peice would you like to move");
                string fromStr = Console.ReadLine().ToUpper();
                if (fromStr.ToLower() == "save")
                {
                    save();
                }

                Console.WriteLine("Where would you like to move to");
                string toStr = Console.ReadLine().ToUpper();

                //commands
                bool back = false;
                if (toStr.ToLower() == "back" || string.IsNullOrEmpty(toStr) || string.IsNullOrEmpty(fromStr))
                {
                    back = true;
                }
                else if (toStr.ToLower() == "save")
                {
                    save();
                }

                if (!back)
                {
                    if (Location.isLocation(fromStr) && Location.isLocation(toStr))
                    {
                        r = new Move(new Location(fromStr), new Location(toStr));
                        List<Move> moves = b.getAllMoves(isWhite);
                        foreach (Move m in moves)
                        {
                            if (m.Equals(r))
                            {
                                invalidEntry = false;
                                break;
                            }
                        }
                    }

                    if (invalidEntry)
                    {
                        Console.WriteLine("Invalid entry");
                    }
                }
            }            

            return r;

        }

        private void save()
        {
            Console.WriteLine("What do you want to call the game");

        }
    }
}
