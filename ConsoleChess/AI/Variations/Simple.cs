using ConsoleChess.AI.Openings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI.Variations
{
    public class Simple
    {
        public Simple(OpeningFileStructure openings, int maxDepth = 4) {
            this.openings = openings;
            this.maxDepth = maxDepth;
        }

        public int maxDepth { get; set; }
        public OpeningFileStructure openings { get; set; }

    }
}
