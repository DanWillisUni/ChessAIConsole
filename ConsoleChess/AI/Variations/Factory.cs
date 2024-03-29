﻿using ConsoleChess.AI.Openings;
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
                    return new Classic(openings, 2, 8);
                case "All1":
                    return new Classic(openings, 1, 0);
                case "All2":
                    return new Classic(openings, 2, 0);
                case "Deep":
                    return new Classic(openings, 7, 8);
                default:
                    Console.WriteLine("Unrecognised type: " + typeStr);
                    break;
            }
            return new Classic(openings);
        }
    }
}
