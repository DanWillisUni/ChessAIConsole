using ConsoleChess.GameRunning;
using ConsoleChess.GameRunning.Player;
using System;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {
            IPlayer p1 = new Human(true);
            IPlayer p2 = new BasicComputer(false);
            Game game = new Game(p1,p2);
        }
    }
}
