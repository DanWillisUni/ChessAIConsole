using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.GameRunning.Player;
using System;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {
            string openingFile = "C:\\Users\\danwi\\repos\\ConsoleChess\\ConsoleChess\\AI\\Openings\\test.txt";
            GenerateOpenings go = new GenerateOpenings(openingFile);
            go.generate();
            Random random = new Random();
            int humanIsWhite = random.Next(2);
            IPlayer human = new Human((humanIsWhite == 0));
            IPlayer computer = new BasicComputer((humanIsWhite == 1), openingFile);
            Game game = (humanIsWhite == 0) ? new Game(human, computer) : new Game(computer, human);
            game.Start();
        }
    }
}
