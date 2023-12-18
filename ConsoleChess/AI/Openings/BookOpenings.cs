using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ConsoleChess.AI.Openings
{
    public class BookOpenings
    {
        public BookOpenings(string boardLine, bool isWhite, List<Move> counters) 
        {
            this.boardLine = boardLine;
            this.isWhite = isWhite;
            this.counters = counters;
        }

        public BookOpenings(Board board, bool isWhite, List<string> counters)
        {
            this.boardLine = board.printAsString();
            this.isWhite = isWhite;
            this.counters = new List<Move>();
            foreach (var counter in counters)
            {
                string[] counterSplit = counter.Split(":");
                this.counters.Add(new Move(new Location(counterSplit[0]), new Location(counterSplit[1])));
            }
        }
        public string boardLine { get; set; }
        public bool isWhite { get; set; }
        public List<Move> counters { get; set; }

        public override string ToString() 
        {
            return boardLine + "!" + JsonSerializer.Serialize(counters);
        }
    }
}
