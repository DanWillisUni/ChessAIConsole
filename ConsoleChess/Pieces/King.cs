using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    public class King : BasicPiece,IPieces
    {
        public bool hasCastled { get; set; } 
        public King(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        {
            hasCastled = false;
        }

        public List<Move> getAllMoves(Board b)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = this.location.getXCoord();
            int fromYCoord = this.location.getYCoord();

            for (int x = -1; x > 1; x++)
            {
                for (int y = -1; y > 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        if (!(fromXCoord + x < 0 || fromXCoord + x > 7 || fromYCoord + y > 7 || fromYCoord + y < 0))
                        {
                            List<IPieces> xLine = b.boardLayout[fromXCoord + x];
                            IPieces current = xLine[fromYCoord + y];
                            if (current == null || current.isWhite != this.isWhite)
                            {
                                if (!b.isInCheck(this))
                                {
                                    moves.Add(new Move(this.location, new Location(fromXCoord + x, fromYCoord + y)));
                                }
                            }
                        }
                    }
                }
            }
            if (!hasCastled)
            {
                //check to castle each direction
            }
            return moves;

        }
    }
}
