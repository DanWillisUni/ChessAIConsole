using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    public class Move
    {
        public Location fromLocation { get; set; }
        public Location toLocation { get; set; }

        public Move (Location from,Location to)
        {
            this.fromLocation = from;
            this.toLocation = to;
        }

        public bool isValid()
        {
            throw new NotImplementedException();
        }
    }
}
