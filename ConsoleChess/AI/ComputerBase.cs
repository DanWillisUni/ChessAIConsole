using ConsoleChess.AI.Model;
using ConsoleChess.AI.Openings;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Collections;
using System.Drawing;

namespace ConsoleChess.AI
{
    public class ComputerBase
    {
        public static Move searchOpenings(OpeningFileStructure openingFileStructure, Board b, bool isWhite)
        {
            Move move = null;
            Console.WriteLine("Searching openings...");
            var lines = File.ReadAllLines(openingFileStructure.getFullOpenings(isWhite));
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineSplit = line.Split("!");
                if (b.printAsString() == lineSplit[0])
                {
                    Console.WriteLine("Match");
                    List<Move> r = JsonSerializer.Deserialize<List<Move>>(lineSplit[1]);
                    Random random = new Random();
                    move = r[random.Next(r.Count)];
                    break;
                }
            }
            return move;
        }

        /**
         * Both sides have no queens or
         * Every side which has a queen has additionally no other pieces or one minorpiece maximum.
         */
        public static bool isEndGame(Board b)
        {
            int queens = 0;
            int minors = 0;
            foreach (IPieces p in b.allPeices)
            {
                char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
                if (type == 'Q')
                {
                    queens++;
                }
                else if (type == 'N' || type == 'B')
                {
                    minors++;
                }
            }
            if (queens == 0 || (queens == 2 && minors <= 1)) {
                return true;
            }
            return false;
        }
        
    }
}
