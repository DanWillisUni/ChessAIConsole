using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    [Serializable]
    public class Move
    {
        public Move() { }
        public Location fromLocation { get; set; }
        public Location toLocation { get; set; }

        public Move (Location from, Location to)
        {
            this.fromLocation = from;
            this.toLocation = to;
        }

        public Move(string s)
        {
            var split = s.Split(':');
            fromLocation = new Location(split[0]);
            toLocation = new Location(split[1]);
        }

        public bool isValid()
        {
            throw new NotImplementedException();
        }

        #region equals
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Move);
        }

        public bool Equals(Move other)
        {
            if (other == null)
                return false;

            return this.fromLocation.Equals(other.fromLocation) && this.toLocation.Equals(other.toLocation);
        }
        #endregion

        public override string ToString() { return "From: " + fromLocation.ToString() + " To: " + toLocation.ToString(); }

        internal Move DeepCopy()
        {
            throw new NotImplementedException();
        }
    }
}
