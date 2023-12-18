using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.AI.Openings
{
    public class OpeningFileStructure
    {
        public string dir {  get; set; }
        public string whiteFileName { get; set; }
        public string blackFileName { get; set; }

        public OpeningFileStructure(string dir, string whiteFileName, string blackFileName)
        {
            this.dir = dir;
            this.whiteFileName = whiteFileName;
            this.blackFileName = blackFileName;
        }

        private string getFullWhitePath() { return dir + whiteFileName; }
        private string getFullBlackPath() {  return dir + blackFileName; }

        public string getFullOpenings(bool isWhite)
        {
            if (isWhite)
            {
                return getFullWhitePath();
            }
            else
            {
                return getFullBlackPath();
            }
        }
    }
}
