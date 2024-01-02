using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using ConsoleChesss;

namespace ConsoleChess.GameRunning
{
    [Serializable]
    public class Board
    {
        public List<IPieces> allPeices { get; set; }
        public  List<Move> pastMoves { get; set; }
        public string[,] layout { get; set; }                

        public Board() : this(false) { }
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

        public Board(List<IPieces> allPeices, List<Move> pastMoves, string[,] layout)
        {
            this.layout = layout;
            this.allPeices = allPeices;
            this.pastMoves = pastMoves;
        }

        private void updateLayout()
        {
            layout = new string[8,8];
            foreach(IPieces p in allPeices)
            {
                layout[p.location.getXCoord(), p.location.getYCoord()] = p.id;
            }
        }
        public void makeMove(Move move)
        {
            IPieces moving = allPeices.Where(o => o.location.Equals(move.fromLocation)).Select(o => o).FirstOrDefault();
            //castling
            if (moving.id[1] == 'K' && moving.numberOfMoves == 0)
            {
                int directionOfTravel = move.fromLocation.getXCoord() - move.toLocation.getXCoord();
                if (Math.Abs(directionOfTravel) > 1) //check if attempting to castle
                {
                    Location rookMoveFrom = null;
                    Location rookMoveTo = null;
                    if (directionOfTravel > 0)
                    {
                        rookMoveFrom = new Location(move.fromLocation.getXCoord() - 4, move.fromLocation.getYCoord());
                        rookMoveTo = new Location(move.toLocation.getXCoord() + 1, move.toLocation.getYCoord());
                    }
                    else if (directionOfTravel < 0)
                    {
                        rookMoveFrom = new Location(move.fromLocation.getXCoord() + 3, move.fromLocation.getYCoord());
                        rookMoveTo = new Location(move.toLocation.getXCoord() - 1, move.toLocation.getYCoord());
                    }
                    if (rookMoveFrom != null && rookMoveTo != null)
                    {
                        IPieces rook = allPeices.Where(o => o.location.Equals(rookMoveFrom)).Select(o => o).FirstOrDefault();
                        allPeices.Remove(rook);
                        rook.numberOfMoves += 1;
                        rook.location = rookMoveTo;
                        allPeices.Add(rook);
                    }
                }
            }
            IPieces taken = allPeices.Where(o => o.location.Equals(move.toLocation)).Select(o => o).FirstOrDefault();
            //taken.location = new Location();            
            allPeices.Remove(taken);
            allPeices.Remove(moving);
            moving.location = move.toLocation;
            moving.numberOfMoves += 1;
            allPeices.Add(moving);
            updateLayout();
            pastMoves.Add(move);
        }

        public void print(bool fromWhitePerspective)
        {
            var whiteForgroud = ConsoleColor.Yellow;
            var blackForgroud = ConsoleColor.DarkBlue;
            var whiteTile = ConsoleColor.Gray;
            var blackTile = ConsoleColor.DarkGray;
            var highlightTile = ConsoleColor.Green;
            List<Location> highlight = pastMoves.Count == 0 ? new List<Location>() : new List<Location>() { pastMoves[pastMoves.Count -1].toLocation,pastMoves[pastMoves.Count - 1].fromLocation };

            List<char> xAxis = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            List<char> yAxis = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8' };
            int yStart = 0;
            int yEnd = 7;
            int yAdd = 1;
            int xStart = 7;
            int xEnd = -1;
            int xAdd = -1;
            var currentTileColour = blackTile;
            if (fromWhitePerspective)
            {
                yStart = 7;
                yEnd = 0;
                yAdd = -1;
                xStart = 0;
                xEnd = 8;
                xAdd = 1;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int x = xStart; x != xEnd; x += xAdd)
            {
                Console.Write("  " + xAxis[x] + "  ");
            }
            Console.WriteLine();

            for (int y = yStart; y != yEnd + yAdd; y += yAdd)
            {
                printBlankLine(currentTileColour,whiteTile,blackTile,y,highlight,highlightTile, fromWhitePerspective);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + yAxis[y] + " ");//axis

                for (int x = xStart; x != xEnd; x += xAdd)
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

                printBlankLine(currentTileColour, whiteTile, blackTile, y, highlight, highlightTile, fromWhitePerspective);

                currentTileColour = (currentTileColour == blackTile ? whiteTile : blackTile);
                Console.BackgroundColor = currentTileColour;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int x = xStart; x != xEnd; x += xAdd)
            {
                Console.Write("  " + xAxis[x] + "  ");
            }
            Console.WriteLine();         
        }
        private void printBlankLine(ConsoleColor currentTileColour, ConsoleColor whiteTile, ConsoleColor blackTile, int y, List<Location> highlight, ConsoleColor highlightTile, bool isWhite)
        {
            int xStart = 7;
            int xEnd = -1;
            int xAdd = -1;
            if (isWhite)
            {
                xStart = 0;
                xEnd = 8;
                xAdd = 1;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");//space for axis on topline
            for (int x = xStart; x != xEnd; x += xAdd)//print the top line  
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

        public List<Move> getAllMoves(bool isWhite, bool includeCastle = true)
        {
            List<IPieces> pieces = (from p in this.allPeices
                                    where p.isWhite == isWhite
                                    select p).ToList();
            List<Move> all = new List<Move>();
            foreach (IPieces p in pieces)
            {
                if (p.id[1] == 'K')
                {
                    King k = (King)p;
                    all.AddRange(BasicPiece.getPossibleMovesKing(k, this, includeCastle));
                }
                else
                {
                    all.AddRange(p.getPossibleMoves(this));
                }
            }
            return all;
        }

        public bool isInCheck(bool isWhite)
        {
            IPieces k = allPeices.Where(o => o.isWhite == isWhite && o.id[1] == 'K').Select(o => o).FirstOrDefault();

            if (k != null)
            {
                return isLocationThreatened(isWhite, k.location);
            }
            return false;
        }

        public bool isLocationThreatened(bool isWhite, Location l)
        {
            char oppColourChar = isWhite ? 'B' : 'W';

            int fromXCoord = l.getXCoord();
            int fromYCoord = l.getYCoord();
            int forwardMultiplyer = (isWhite ? 1 : -1);

            //knight
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    if (x != 0 && y != 0 && Math.Pow(x, 2) != Math.Pow(y, 2) && Board.isOnBoard(fromXCoord + x, fromYCoord + y))
                    {
                        string current = layout[fromXCoord + x, fromYCoord + y];
                        if (!String.IsNullOrEmpty(current))
                        {
                            if (current[0] == oppColourChar && current[1] == 'N')
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            List<bool> results = new List<bool>();
            //queen and bishop
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, -1, -1, oppColourChar, new List<char> { 'Q', 'B' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, -1, 1, oppColourChar, new List<char> { 'Q', 'B' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, 1, 1, oppColourChar, new List<char> { 'Q', 'B' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, 1, -1, oppColourChar, new List<char> { 'Q', 'B' }));

            //rook and queen
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, -1, 0, oppColourChar, new List<char> { 'Q', 'R' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, 1, 0, oppColourChar, new List<char> { 'Q', 'R' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, 0, 1, oppColourChar, new List<char> { 'Q', 'R' }));
            results.Add(isInCheckDirection(fromXCoord, fromYCoord, 0, -1, oppColourChar, new List<char> { 'Q', 'R' }));
            if (results.Contains(true))
            {
                return true;
            }

            //pawn
            for (int x = -1; x < 2; x += 2)
            {
                if (Board.isOnBoard(fromXCoord + x, fromYCoord + forwardMultiplyer))
                {
                    if (!String.IsNullOrEmpty(layout[fromXCoord + x, fromYCoord + forwardMultiplyer]))
                    {
                        string current = layout[fromXCoord + x, fromYCoord + forwardMultiplyer];
                        if (current[0] == oppColourChar && current[1] == 'P')
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool isInCheckDirection(int fromXCoord, int fromYCoord, int x, int y, char oppColourChar, List<char> leathalPeices)
        {
            bool canContinue = true;
            int count = 0;
            do
            {
                count++;
                if (!Board.isOnBoard(fromXCoord + (x * count), fromYCoord + (y * count)))
                {
                    canContinue = false;
                }
                else
                {
                    string current = layout[fromXCoord + (x * count), fromYCoord + (y * count)];
                    if (!String.IsNullOrEmpty(current))
                    {
                        if (current[0] == oppColourChar && leathalPeices.Contains(current[1]))
                        {
                            return true;
                        }
                        canContinue = false;
                    }
                }

            } while (canContinue);
            return false;
        }

        public bool isCheckmate(bool isWhitesTurn)
        {
            if (isInCheck(isWhitesTurn))
            {
                List<Move> allMoves = this.getAllMoves(isWhitesTurn);
                List<Move> checkMoves = BasicPiece.removeInCheck(allMoves, isWhitesTurn, this);
                if (checkMoves.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isStalemate(bool isWhitesTurn)
        {
            //turn but no legal moves
            List<Move> allMoves = this.getAllMoves(isWhitesTurn);
            List<Move> checkMoves = BasicPiece.removeInCheck(allMoves, isWhitesTurn, this);
            if (checkMoves.Count == 0)
            {
                return true;
            }
            //insuficient material
            //repeat moves
            return false;
        }
        public string printAsString()
        {
            updateLayout();
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

        public Board DeepCopy()
        {
            Board deepcopy = new Board(true);
            foreach(var piece in this.allPeices)
            {
                deepcopy.allPeices.Add(piece.DeepCopy());
            }
            foreach(var move in this.pastMoves)
            {
                deepcopy.pastMoves.Add(move.DeepCopy());
            }
            deepcopy.updateLayout();
            return deepcopy;
        }

        public static bool isOnBoard(int x, int y)
        {
            return (x <= 7 && x >= 0 && y <= 7 && y >= 0);
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
        public void addPeice(char type, bool isWhite, string location)
        {
            addPeice(generateID(type, isWhite), new Location(location));
        }
        public string generateID(char type, bool isWhite)
        {
            string r = isWhite ? "W" : "B";
            int countType = 1;
            foreach (IPieces p in allPeices)
            {
                if (p.id[1] == type && p.id[0].ToString() == r)
                {
                    countType++;
                }
            }
            r += type;
            r += countType.ToString();
            return r;
        }
    }
}
