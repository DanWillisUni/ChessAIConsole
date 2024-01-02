using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.GameRunning.Player;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootDir = "ChessFilesRoot\\";
            string gameDir = "Games\\";
            OpeningFileStructure openingFileStructure = initOpenings(rootDir);

            Random random = new Random();
            int humanIsWhite = random.Next(2);
            IPlayer human = new Human((humanIsWhite == 0));
            string gameType = "Blitz";//intro(); 
            IPlayer computer = new BasicComputer((humanIsWhite == 1), openingFileStructure, gameType);
            Game game = (humanIsWhite == 0) ? new Game(human, computer) : new Game(computer, human);
            runGame(game, rootDir, gameDir);
        }

        private static void runGame(Game g, string rootDir, string gameDir)
        {
            string command = g.run();
            if (command == "SAVE")
            {
                save(rootDir, gameDir, g);
            }
            else if (command == "LOAD")
            {
                Game game = load(rootDir, gameDir);
                runGame(game, rootDir, gameDir);
            }
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
            Console.WriteLine("Which gamemode would you like to play [Blitz, Deep]");
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

        private static void save(string rootDir, string gameDir, Game g)
        {
            if (!Directory.Exists(rootDir + gameDir))
            {
                Directory.CreateDirectory(rootDir + gameDir);
            }

            //get name
            string gameName = "";
            while (String.IsNullOrEmpty(gameName))
            {
                Console.WriteLine("What do you want to call the game");
                gameName = Console.ReadLine();

                if (File.Exists(rootDir + gameDir + gameName + ".txt"))
                {
                    string yorn = "";
                    List<string> acceptedAnswers = new List<string> { "y", "n", "yes", "no" };
                    while (!acceptedAnswers.Contains(yorn.ToLower()))
                    {
                        Console.WriteLine("Do you wish to overwrite existing file? (y/n)");
                        yorn = Console.ReadLine();
                    }
                    if (yorn.ToLower().StartsWith('n'))
                    {
                        gameName = "";
                    }
                }
            }

            string filePath = rootDir + gameDir + gameName + ".txt";

            g.save(filePath);
        }

        private static Game load(string rootDir, string gameDir)
        {
            if (!Directory.Exists(rootDir + gameDir))
            {
                Directory.CreateDirectory(rootDir + gameDir);
            }

            //get name
            string gameName = "";
            while (String.IsNullOrEmpty(gameName))
            {
                Console.WriteLine("What is the game called");
                gameName = Console.ReadLine();

                if (!File.Exists(rootDir + gameDir + gameName + ".txt"))
                {
                    Console.WriteLine($"{gameName} does not exist");
                    gameName = "";
                }
            }

            string filePath = rootDir + gameDir + gameName + ".txt";

            Game r = Game.load(filePath);
            return r;
        }
    }
}
