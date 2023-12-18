using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace ConsoleChess.AI.Openings
{
    public class GenerateOpenings
    {
        public GenerateOpenings(string fileLocation, string whiteFileName, string blackFileName) 
        {
            this.fileLocation = fileLocation;
            this.whiteFileName = whiteFileName;
            this.blackFileName = blackFileName;
        }

        private string fileLocation {  get; set; }
        private string whiteFileName { get; set; }
        private string blackFileName { get; set; }

        public void generate()
        {
            //https://database.chessbase.com/
            List<BookOpenings> bookOpenings = new List<BookOpenings>();
            Board blank = new Board();
            bookOpenings.Add(new BookOpenings(blank, true, new List<string> { "E2:E4", "D2:D4" }));

            #region E4
            Board e4 = blank.DeepCopy();
            e4.makeMove(new Move("E2:E4"));
            bookOpenings.Add(new BookOpenings(e4, false, new List<string> { "E7:E5", "C7:C5", "E7:E6" }));

            Board e4e5 = e4.DeepCopy();
            e4e5.makeMove(new Move("E7:E5"));
            bookOpenings.Add(new BookOpenings(e4e5, true, new List<string> { "G1:F3" }));

            Board e4e5f3 = e4e5.DeepCopy();
            e4e5f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(e4e5f3, false, new List<string> { "B8:C6" }));

            Board e4e5f3c6 = e4e5f3.DeepCopy();
            e4e5f3c6.makeMove(new Move("B8:C6"));
            bookOpenings.Add(new BookOpenings(e4e5f3c6, true, new List<string> { "F1:B5", "F1:C4" }));

            Board e4e5f3c6b5 = e4e5f3c6.DeepCopy();
            e4e5f3c6b5.makeMove(new Move("F1:B5"));
            bookOpenings.Add(new BookOpenings(e4e5f3c6b5, false, new List<string> { "A7:A6", "G8:F6" }));

            Board e4e5f3c6c4 = e4e5f3c6.DeepCopy();
            e4e5f3c6c4.makeMove(new Move("F1:C4"));
            bookOpenings.Add(new BookOpenings(e4e5f3c6c4, false, new List<string> { "F8:C5", "G8:F6" }));


            Board e4e6 = e4.DeepCopy();
            e4e6.makeMove(new Move("E7:E6"));
            bookOpenings.Add(new BookOpenings(e4e6, true, new List<string> { "D2:D4" }));

            Board e4e6d4 = e4e6.DeepCopy();
            e4e6d4.makeMove(new Move("D2:D4"));
            bookOpenings.Add(new BookOpenings(e4e6d4, false, new List<string> { "D7:D5" }));

            Board e4e6d4d5 = e4e6d4.DeepCopy();
            e4e6d4d5.makeMove(new Move("D7:D5"));
            bookOpenings.Add(new BookOpenings(e4e6d4d5, true, new List<string> { "B1:C3", "B1:D2" }));

            Board e4e6d4d5c3 = e4e6d4d5.DeepCopy();
            e4e6d4d5c3.makeMove(new Move("B1:C3"));
            bookOpenings.Add(new BookOpenings(e4e6d4d5c3, false, new List<string> { "G8:F6", "F8:B4" }));

            Board e4e6d4d5d2 = e4e6d4d5.DeepCopy();
            e4e6d4d5d2.makeMove(new Move("B1:D2"));
            bookOpenings.Add(new BookOpenings(e4e6d4d5d2, false, new List<string> { "C7:C5", "G8:F6" }));


            Board e4c5 = e4.DeepCopy();
            e4c5.makeMove(new Move("C7:C5"));
            bookOpenings.Add(new BookOpenings(e4c5, true, new List<string> { "G1:F3" }));

            Board e4c5f3 = e4c5.DeepCopy();
            e4c5f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(e4c5f3, false, new List<string> { "D7:D6", "B8:C6", "E7:E6" }));

            Board e4c5f3d6 = e4c5f3.DeepCopy();
            e4c5f3d6.makeMove(new Move("D7:D6"));
            bookOpenings.Add(new BookOpenings(e4c5f3d6, true, new List<string> { "D2:D4" }));

            Board e4c5f3d6d4 = e4c5f3d6.DeepCopy();
            e4c5f3d6d4.makeMove(new Move("D2:D4"));
            bookOpenings.Add(new BookOpenings(e4c5f3d6d4, false, new List<string> { "C5:D4" }));

            Board e4c5f3c6 = e4c5f3.DeepCopy();
            e4c5f3c6.makeMove(new Move("B8:C6"));
            bookOpenings.Add(new BookOpenings(e4c5f3c6, true, new List<string> { "D2:D4", "F1:B5" }));

            Board e4c5f3c6d4 = e4c5f3c6.DeepCopy();
            e4c5f3c6d4.makeMove(new Move("D2:D4"));
            bookOpenings.Add(new BookOpenings(e4c5f3c6d4, false, new List<string> { "C5:D4" }));

            Board e4c5f3c6b5 = e4c5f3c6.DeepCopy();
            e4c5f3c6b5.makeMove(new Move("F1:B5"));
            bookOpenings.Add(new BookOpenings(e4c5f3c6b5, false, new List<string> { "G7:G6", "E7:E6", "D7:D6" }));

            Board e4c5f3e6 = e4c5f3.DeepCopy();
            e4c5f3e6.makeMove(new Move("E7:E6"));
            bookOpenings.Add(new BookOpenings(e4c5f3e6, true, new List<string> { "D2:D4" }));

            Board e4c5f3e6d4 = e4c5f3e6.DeepCopy();
            e4c5f3e6d4.makeMove(new Move("D2:D4"));
            bookOpenings.Add(new BookOpenings(e4c5f3e6d4, false, new List<string> { "C5:D4" }));
            #endregion

            #region D4
            Board d4 = blank.DeepCopy();
            d4.makeMove(new Move("D2:D4"));
            bookOpenings.Add(new BookOpenings(d4, false, new List<string> { "G8:F6", "D7:D5" }));

            Board d4f6 = d4.DeepCopy();
            d4f6.makeMove(new Move("G8:F6"));
            bookOpenings.Add(new BookOpenings(d4f6, true, new List<string> { "C2:C4", "G1:F3" }));

            Board d4f6c4 = d4f6.DeepCopy();
            d4f6c4.makeMove(new Move("C2:C4"));
            bookOpenings.Add(new BookOpenings(d4f6c4, false, new List<string> { "E7:E6", "G7:G6" }));

            Board d4f6c4e6 = d4f6c4.DeepCopy();
            d4f6c4e6.makeMove(new Move("E7:E6"));
            bookOpenings.Add(new BookOpenings(d4f6c4e6, true, new List<string> { "G1:F3", "B1:C3" }));

            Board d4f6c4e6f3 = d4f6c4e6.DeepCopy();
            d4f6c4e6f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(d4f6c4e6f3, false, new List<string> { "D7:D5"  }));

            Board d4f6c4e6c3 = d4f6c4e6.DeepCopy();
            d4f6c4e6c3.makeMove(new Move("B1:C3"));
            bookOpenings.Add(new BookOpenings(d4f6c4e6c3, false, new List<string> { "D7:D5", "F8:B4" }));

            Board d4f6c4g6 = d4f6c4.DeepCopy();
            d4f6c4g6.makeMove(new Move("G7:G6"));
            bookOpenings.Add(new BookOpenings(d4f6c4g6, true, new List<string> { "B1:C3" }));

            Board d4f6c4g6c3 = d4f6c4g6.DeepCopy();
            d4f6c4g6c3.makeMove(new Move("B1:C3"));
            bookOpenings.Add(new BookOpenings(d4f6c4g6c3, false, new List<string> { "F8:G7", "D7:D5" }));

            Board d4f6f3 = d4f6.DeepCopy();
            d4f6f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(d4f6f3, false, new List<string> { "D7:D5" }));

            Board d4f6f3d5 = d4f6f3.DeepCopy();
            d4f6f3d5.makeMove(new Move("D7:D5"));
            bookOpenings.Add(new BookOpenings(d4f6f3d5, true, new List<string> { "C2:C4", "C1:F4" }));

            Board d4f6f3d5c4 = d4f6f3d5.DeepCopy();
            d4f6f3d5c4.makeMove(new Move("C2:C4"));
            bookOpenings.Add(new BookOpenings(d4f6f3d5c4, false, new List<string> { "E7:E6", "C7:C6" }));

            Board d4f6f3d5f4 = d4f6f3d5.DeepCopy();
            d4f6f3d5f4.makeMove(new Move("C1:F4"));
            bookOpenings.Add(new BookOpenings(d4f6f3d5f4, false, new List<string> { "C7:C5", "E7:E6" }));

            Board d4d5 = d4.DeepCopy();
            d4d5.makeMove(new Move("D7:D5"));
            bookOpenings.Add(new BookOpenings(d4d5, true, new List<string> { "C2:C4", "G1:F3" }));

            Board d4d5c4 = d4d5.DeepCopy();
            d4d5c4.makeMove(new Move("C2:C4"));
            bookOpenings.Add(new BookOpenings(d4d5c4, false, new List<string> { "E7:E6", "C7:C6" }));

            Board d4d5c4e6 = d4d5c4.DeepCopy();
            d4d5c4e6.makeMove(new Move("E7:E6"));
            bookOpenings.Add(new BookOpenings(d4d5c4e6, true, new List<string> { "B1:C3", "G1:F3" }));

            Board d4d5c4e6c3 = d4d5c4e6.DeepCopy();
            d4d5c4e6c3.makeMove(new Move("B1:C3"));
            bookOpenings.Add(new BookOpenings(d4d5c4e6c3, false, new List<string> { "G8:F6", "C7:C6" }));

            Board d4d5c4e6f3 = d4d5c4e6.DeepCopy();
            d4d5c4e6f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(d4d5c4e6f3, false, new List<string> { "G8:F6" }));

            Board d4d5f3 = d4d5.DeepCopy();
            d4d5f3.makeMove(new Move("G1:F3"));
            bookOpenings.Add(new BookOpenings(d4d5f3, false, new List<string> { "G8:F6" }));

            Board d4d5f3f6 = d4d5f3.DeepCopy();
            d4d5f3f6.makeMove(new Move("G8:F6"));
            bookOpenings.Add(new BookOpenings(d4d5f3f6, true, new List<string> { "C2:C4", "C1:F4" }));

            Board d4d5f3f6c4 = d4d5f3f6.DeepCopy();
            d4d5f3f6c4.makeMove(new Move("C2:C4"));
            bookOpenings.Add(new BookOpenings(d4d5f3f6c4, false, new List<string> { "E7:E6", "C7:C6" }));

            Board d4d5f3f6f4 = d4d5f3f6.DeepCopy();
            d4d5f3f6f4.makeMove(new Move("C1:F4"));
            bookOpenings.Add(new BookOpenings(d4d5f3f6f4, false, new List<string> { "C7:C5", "E7:E6" }));

            #endregion


            publish(bookOpenings);
        }

        private void publish(List<BookOpenings> bookOpenings)
        {
            if (!Directory.Exists(this.fileLocation))
            {
                Directory.CreateDirectory(this.fileLocation);
            }
            List<string> whiteLines = new List<string>();
            List<string> blackLines = new List<string>();
            foreach (var book in bookOpenings)
            {
                if (book.isWhite)
                {
                    whiteLines.Add(book.ToString());
                }
                else
                {
                    blackLines.Add(book.ToString());
                }
            }

            publishOne(this.fileLocation + this.whiteFileName, whiteLines);
            publishOne(this.fileLocation + this.blackFileName, blackLines);
        }

        private void publishOne(string file, List<string> lines)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            FileStream fs = File.Create(file);
            fs.Close();

            using (TextWriter tw = new StreamWriter(file))
            {
                foreach (string s in lines)
                {
                    tw.WriteLine(s);
                }
            }
        }
    }
}
