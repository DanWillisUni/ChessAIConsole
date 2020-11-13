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

        public Location(int xCoord,int yCoord)
        {
            XLocation = (char)(xCoord + 65);
            YLocation = (char)(yCoord + 49);
        }

        public int getXCoord()
        {
            return Convert.ToInt32(XLocation) - 65;
        }
        public int getYCoord()
        {
            return Convert.ToInt32(YLocation) - 49;
        }
    }
}
