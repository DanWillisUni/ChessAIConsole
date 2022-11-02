using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    [Serializable]
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
        public Location(string s) : this(s[0], s[1]) { }  
        
        public Location() : this(0,0) { }

        public int getXCoord()
        {
            return Convert.ToInt32(XLocation) - 65;
        }
        public int getYCoord()
        {
            return Convert.ToInt32(YLocation) - 49;
        }


        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public bool Equals(Location other)
        {
            if (other == null)
                return false;

            return this.XLocation.Equals(other.XLocation) && this.YLocation.Equals(other.YLocation);
        }
    }
}
