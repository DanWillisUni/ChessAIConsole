using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleChess.GameRunning
{
    [Serializable]
    public class Board
    {
        public List<IPieces> allPeices { get; set; }
        public  List<Move> pastMoves { get; set; }
        public string[,] layout { get; set; }                

        public Board(bool clean=false)
        {
            allPeices = new List<IPieces>();
            if (!clean)
            {
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
            }
            updateLayout();
            pastMoves = new List<Move>();
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
            pastMoves.Add(move);
        }

        public void print(bool fromWhitePerspective)
        {
            var whiteForgroud = ConsoleColor.Magenta;
            var blackForgroud = ConsoleColor.Cyan;
            var whiteTile = ConsoleColor.White;
            var blackTile = ConsoleColor.Black;
            var highlightTile = ConsoleColor.Green;
            List<Location> highlight = pastMoves.Count == 0 ? new List<Location>() : new List<Location>() { pastMoves[pastMoves.Count -1].toLocation,pastMoves[pastMoves.Count - 1].fromLocation };

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
                printBlankLine(currentTileColour,whiteTile,blackTile,y,highlight,highlightTile);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + yAxis[y] + " ");//axis

                for (int x = 0; x != 8; x += 1)
                {
                    currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                    Console.BackgroundColor = currentTileColour;
                    foreach(Location l in highlight)
                    {
                        if(l.getXCoord() == x && l.getYCoord() == y)
                        {
                            Console.BackgroundColor = highlightTile;
                            break;
                        }
                    }

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

                printBlankLine(currentTileColour, whiteTile, blackTile, y, highlight, highlightTile);

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
        private void printBlankLine(ConsoleColor currentTileColour, ConsoleColor whiteTile, ConsoleColor blackTile, int y, List<Location> highlight, ConsoleColor highlightTile)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");//space for axis on topline
            for (int x = 0; x != 8; x += 1)//print the top line
            {
                currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                Console.BackgroundColor = currentTileColour;
                foreach (Location l in highlight)
                {
                    if (l.getXCoord() == x && l.getYCoord() == y)
                    {
                        Console.BackgroundColor = highlightTile;
                        break;
                    }
                }                
                Console.Write("     ");                
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        public List<Move> getAllMoves(bool isWhite)
        {
            List<IPieces> pieces = (from p in this.allPeices
                                    where p.isWhite == isWhite
                                    select p).ToList();
            List<Move> all = new List<Move>();
            foreach (IPieces p in pieces)
            {
                all.AddRange(p.getPossibleMoves(this));
            }
            return all;
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

        public string printAsString()
        {
            string r = "";
            for (int y = 7; y != -1; y -= 1)
            {
                for (int x = 0; x != 8; x += 1)
                {
                    if (layout[x, y] == null)
                    {
                        r += " ";
                    }
                    else
                    {
                        Location l = new Location(x, y);
                        IPieces current = this.allPeices.Where(o => o.location.XLocation == l.XLocation && o.location.YLocation == l.YLocation).Select(o => o).FirstOrDefault();
                        string toWrite = current.id[1] != 'P' ? current.id[1].ToString() : ((Pawn)current).moveType.ToString();
                        if (current.isWhite)
                        {
                            r += toWrite.ToUpper();
                        }
                        else
                        {
                            r += toWrite.ToLower();
                        }
                    }
                }
            }
            return r;
        }

        // For Unit tests
        public void addPeice(string id, Location location)
        {
            bool isWhite = id.ToUpper()[0] == 'W' ? true : false;
            IPieces newOne = null;
            switch (id.ToUpper()[1])
            {
                case 'P':
                    newOne = new Pawn(id.ToUpper(), 0, location);
                    break;
                case 'R':
                    newOne = new Rook(id.ToUpper(), 0, location);
                    break;
                case 'N':
                    newOne = new Knight(id.ToUpper(), 0, location);
                    break;
                case 'B':
                    newOne = new Bishop(id.ToUpper(), 0, location);
                    break;
                case 'Q':
                    newOne = new Queen(id.ToUpper(), 0, location);
                    break;
                case 'K':
                    newOne = new King(id.ToUpper(), 0, location);
                    break;
                default:
                    break;
            }
            allPeices.Add(newOne);
            updateLayout();
        }
    }
}
