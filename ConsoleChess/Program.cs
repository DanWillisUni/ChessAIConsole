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
            string rootDir = "ChessFilesRoot\\";
            OpeningFileStructure openingFileStructure = initOpenings(rootDir);

            Random random = new Random();
            int humanIsWhite = random.Next(2);
            IPlayer human = new Human((humanIsWhite == 0));
            string gameType = "Blitz";//intro(); 
            IPlayer computer = new BasicComputer((humanIsWhite == 1), openingFileStructure, gameType);
            Game game = (humanIsWhite == 0) ? new Game(human, computer) : new Game(computer, human);
            game.Start();
        }

        private static string intro()
        {
            Console.WriteLine("P = Pawn");
            Console.WriteLine("R = Rook");
            Console.WriteLine("N = Knight");
            Console.WriteLine("B = Bishop");
            Console.WriteLine("Q = Queen");
            Console.WriteLine("K = King");
            Console.WriteLine("CheckMate to Win");
            Console.WriteLine("FiveFold, Stalemate and Insufficient Material Draw are the ways to draw");
            Console.WriteLine();
            Console.WriteLine("Which game would you like to play [Blitz, Deep]");
            string r = Console.ReadLine();
            return r;
        }

        private static OpeningFileStructure initOpenings(string rootDir)
        {
            string openingsDir = rootDir + "Openings\\";
            string openingsWhiteFile = "whiteOpenings.txt";
            string openingsBlackFile = "blackOpenings.txt";
            GenerateOpenings go = new GenerateOpenings(openingsDir, openingsWhiteFile, openingsBlackFile);
            go.generate();
            OpeningFileStructure openingFileStructure = new OpeningFileStructure(openingsDir, openingsWhiteFile, openingsBlackFile);
            return openingFileStructure;
        }
    }
}
