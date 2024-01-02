using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;
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

        public MoveReturn makeTurn(Board b)
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
                    return new MoveReturn("SAVE", null);
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
                    return new MoveReturn("SAVE", null);
                }

                if (!back)
                {
                    if (Location.isLocation(fromStr) && Location.isLocation(toStr))
                    {
                        r = new Move(new Location(fromStr), new Location(toStr));
                        List<Move> moves = b.getAllMoves(isWhite);
                        moves = BasicPiece.removeInCheck(moves, isWhite, b); //faster for computer to just do this instead
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

            return new MoveReturn("", r);
        }
    }
}
