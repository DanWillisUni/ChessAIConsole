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
            IPlayer computer = new BasicComputer((humanIsWhite == 1), openingFileStructure);
            Game game = (humanIsWhite == 0) ? new Game(human, computer) : new Game(computer, human);
            game.Start();
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
