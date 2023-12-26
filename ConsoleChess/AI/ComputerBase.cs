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

namespace ConsoleChess.AI
{
    public class ComputerBase
    {
        public static readonly Dictionary<Char, Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 1 }, { 'B', 3 }, { 'N', 3 }, { 'R', 5 }, { 'Q', 9 }, { 'K', 90 } };
        
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
        public static int getPeiceValue(IPieces p)
        {
            char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
            return peiceValues[type];
        }

        public static double getValueOfThreatening(Board b, List<Move> moves)
        {
            double r = 0;
            foreach (Move move in moves)
            {
                IPieces taken = b.allPeices.Where(o => o.location.Equals(move.toLocation) && o.id.ToUpper()[1] != 'K').Select(o => o).FirstOrDefault();
                if (taken != null)
                {
                    r += ComputerBase.getPeiceValue(taken);
                }
            }
            return r;
        }
    }
}
