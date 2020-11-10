using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    public class Location
    {
        public char XLocation { get; set; }
        public char YLocation { get; set; }

        public Location(char XLocation,char YLocation)
        {
            this.XLocation = XLocation;
            this.YLocation = YLocation;
        }
    }
}
