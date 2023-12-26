using ConsoleChess.AI.Openings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI.Variations
{
    public class Factory
    {
        public Factory(bool isWhite) { this.isWhite = isWhite; }
        public bool isWhite { get; set; }

        public IComputer generateComputer(string typeStr, OpeningFileStructure openings)
        {
            switch (typeStr) {
                case "Blitz":
                    return new Classic(openings, 1, 5);
                default:
                    Console.WriteLine("Unrecognised type: " + typeStr);
                    break;
            }
            return new Classic(openings, 1, 5);
        }
    }
}
