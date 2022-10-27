using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleChess.GameRunning
{
    public class Board
    {
        public List<IPieces> allPeices { get; set; }
        public string[,] layout { get; set; }

        public Board(bool isWhite)
        {
            throw new NotImplementedException();
        } 

        public Board()
        {
            allPeices = new List<IPieces>();
            allPeices.Add(new Rook("BR2", 0, new Location('A', '8')));
            allPeices.Add(new Pawn("BP8", 0, new Location('A', '7')));
            allPeices.Add(new Pawn("WP1", 0, new Location('A', '2')));
            allPeices.Add(new Rook("WR1", 0, new Location('A', '1')));
            allPeices.Add(new Knight("BN2", 0, new Location('B', '8')));
            allPeices.Add(new Pawn("BP7", 0, new Location('B', '7')));
            allPeices.Add(new Pawn("WP2", 0, new Location('B', '2')));
            allPeices.Add(new Knight("WN1", 0, new Location('B', '1')));
            allPeices.Add(new Bishop("BB2", 0, new Location('C', '8')));
            allPeices.Add(new Pawn("BP6", 0, new Location('C', '7')));
            allPeices.Add(new Pawn("WP3", 0, new Location('C', '2')));
            allPeices.Add(new Bishop("WB1", 0, new Location('C', '1')));
            allPeices.Add(new Queen("BQ", 0, new Location('D', '8')));
            allPeices.Add(new Pawn("BP5", 0, new Location('D', '7')));
            allPeices.Add(new Pawn("WP4", 0, new Location('D', '2')));
            allPeices.Add(new Queen("WQ", 0, new Location('D', '1')));
            allPeices.Add(new King("BK", 0, new Location('E', '8')));
            allPeices.Add(new Pawn("BP4", 0, new Location('E', '7')));
            allPeices.Add(new Pawn("WP5", 0, new Location('E', '2')));
            allPeices.Add(new King("WK", 0, new Location('E', '1')));
            allPeices.Add(new Bishop("BB1", 0, new Location('F', '8')));
            allPeices.Add(new Pawn("BP3", 0, new Location('F', '7')));
            allPeices.Add(new Pawn("WP6", 0, new Location('F', '2')));
            allPeices.Add(new Bishop("WB2", 0, new Location('F', '1')));
            allPeices.Add(new Knight("BN1", 0, new Location('G', '8')));
            allPeices.Add(new Pawn("BP2", 0, new Location('G', '7')));
            allPeices.Add(new Pawn("WP7", 0, new Location('G', '2')));
            allPeices.Add(new Knight("WN2", 0, new Location('G', '1')));
            allPeices.Add(new Rook("BR1", 0, new Location('H', '8')));
            allPeices.Add(new Pawn("BP1", 0, new Location('H', '7')));
            allPeices.Add(new Pawn("WP8", 0, new Location('H', '2')));
            allPeices.Add(new Rook("WR2", 0, new Location('H', '1')));
            updateLayout();
        }

        private void updateLayout()
        {
            layout = new string[8,8];
            foreach(IPieces p in allPeices)
            {
                layout[p.location.getXCoord(),p.location.getYCoord()] = p.id;
            }
        }
        public void makeMove(Move move)
        {
            IPieces moving = allPeices.Where(o => o.location.Equals(move.fromLocation)).Select(o => o).FirstOrDefault();
            IPieces taken = allPeices.Where(o => o.location.Equals(move.toLocation)).Select(o => o).FirstOrDefault();
            //taken.location = new Location();            
            allPeices.Remove(taken);
            allPeices.Remove(moving);
            moving.location = move.toLocation;
            allPeices.Add(moving);
            updateLayout();
        }

        public void print(bool fromWhitePerspective)
        {
            var whiteForgroud = ConsoleColor.Magenta;
            var blackForgroud = ConsoleColor.Cyan;
            var whiteTile = ConsoleColor.White;
            var blackTile = ConsoleColor.Black;

            List<char> xAxis = new List<char>() {'A','B','C','D','E','F','G','H'};
            List<char> yAxis = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8' };
            int yStart = 0;
            int yEnd = 7;
            int yAdd = 1;
            var currentTileColour = whiteTile;
            if (fromWhitePerspective)
            {
                yStart = 7;
                yEnd = 0;
                yAdd = -1;
                currentTileColour = blackTile;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int i = 0; i != 8; i += 1)
            {
                Console.Write("  " + xAxis[i] + "  ");
            }
            Console.WriteLine();

            for (int y = yStart; y != yEnd + yAdd; y += yAdd)
            {
                printBlankLine(currentTileColour,whiteTile,blackTile);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + yAxis[y] + " ");//axis

                for (int x = 0; x != 8; x += 1)
                {
                    currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                    Console.BackgroundColor = currentTileColour;

                    if (layout[x, y] == null)
                    {
                        Console.Write("     ");
                    }
                    else
                    {
                        Location l = new Location(x, y);
                        IPieces current = this.allPeices.Where(o => o.location.XLocation == l.XLocation && o.location.YLocation == l.YLocation).Select(o => o).FirstOrDefault();
                        string toWrite = current.id[1] != 'P' ? current.id[1].ToString(): ((Pawn)current).moveType.ToString();                                                
                        Console.ForegroundColor = current.isWhite ? whiteForgroud : blackForgroud;    
                        Console.Write("  " + toWrite + "  ");
                    }                    
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" " + yAxis[y] + " ");//axis

                printBlankLine(currentTileColour, whiteTile, blackTile);

                currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                Console.BackgroundColor = currentTileColour;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int i = 0; i != 8; i += 1)
            {
                Console.Write("  " + xAxis[i] + "  ");
            }
            Console.WriteLine();         
        }
        private void printBlankLine(ConsoleColor currentTileColour, ConsoleColor whiteTile, ConsoleColor blackTile)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");//space for axis on topline
            for (int topX = 0; topX != 8; topX += 1)//print the top line
            {
                currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                Console.BackgroundColor = currentTileColour;

                if (topX == 7)
                {
                    Console.WriteLine("     ");
                }
                else
                {
                    Console.Write("     ");
                }
            }
        }
        
        internal bool isInCheck(King king)
        {/*
            for(int y = 0; y > 7; y++)
            {
                for (int x = 0; x > 7; x++)
                {
                    IPieces current = allPeices[x][y];
                    if (current.isWhite != king.isWhite)
                    {
                        List<Move> currentMoves = current.getAllMoves(this);
                        foreach(Move m in currentMoves)
                        {
                            if(m.toLocation == king.location)
                            {
                                return true;
                            }
                        }
                    }
                }
            }*/
            return false;
        }
        
    }
}
