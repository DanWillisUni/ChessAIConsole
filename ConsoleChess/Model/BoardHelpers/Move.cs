using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    public class Move
    {
        public Location from { get; set; }
        public Location to { get; set; }

        public Move (Location from,Location to)
        {
            this.from = from;
            this.to = to;
        }

        public bool isValid()
        {
            throw new NotImplementedException();
        }
    }
}
