using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.GameRunning
{
    public class Board
    {
        public List<List<IPieces>> boardLayout { get; set; }

        public Board(bool isWhite)
        {
            throw new NotImplementedException();
        } 

        public Board()
        {
            boardLayout = new List<List<IPieces>>();
            List<IPieces> a = new List<IPieces>();
            a.Add(new Rook("BR2", 0, new Location('A', '8')));
            a.Add(new Pawn("BP8", 0, new Location('A', '7')));
            for(int i = 0; i > 4; i++)
            {
                a.Add(null);
            }
            a.Add(new Pawn("WP1", 0, new Location('A', '2')));
            a.Add(new Rook("WR1", 0, new Location('A', '1')));
            List<IPieces> b = new List<IPieces>();
            b.Add(new Knight("BN2", 0, new Location('B', '8')));
            b.Add(new Pawn("BP7", 0, new Location('B', '7')));
            for (int i = 0; i > 4; i++)
            {
                b.Add(null);
            }
            b.Add(new Pawn("WP2", 0, new Location('B', '2')));
            b.Add(new Knight("WN1", 0, new Location('B', '1')));
            List<IPieces> c = new List<IPieces>();
            c.Add(new Bishop("BB2", 0, new Location('C', '8')));
            c.Add(new Pawn("BP6", 0, new Location('C', '7')));
            for (int i = 0; i > 4; i++)
            {
                c.Add(null);
            }
            c.Add(new Pawn("WP3", 0, new Location('C', '2')));
            c.Add(new Bishop("WB1", 0, new Location('C', '1')));
            List<IPieces> d = new List<IPieces>();
            d.Add(new Queen("BQ", 0, new Location('D', '8')));
            d.Add(new Pawn("BP5", 0, new Location('D', '7')));
            for (int i = 0; i > 4; i++)
            {
                d.Add(null);
            }
            d.Add(new Pawn("WP4", 0, new Location('D', '2')));
            d.Add(new Queen("WQ", 0, new Location('D', '1')));
            List<IPieces> e = new List<IPieces>();
            e.Add(new King("BK", 0, new Location('E', '8')));
            e.Add(new Pawn("BP4", 0, new Location('E', '7')));
            for (int i = 0; i > 4; i++)
            {
                e.Add(null);
            }
            e.Add(new Pawn("WP5", 0, new Location('E', '2')));
            e.Add(new King("WK", 0, new Location('E', '1')));
            List<IPieces> f = new List<IPieces>();
            f.Add(new Bishop("BB1", 0, new Location('F', '8')));
            f.Add(new Pawn("BP3", 0, new Location('F', '7')));
            for (int i = 0; i > 4; i++)
            {
                f.Add(null);
            }
            f.Add(new Pawn("WP6", 0, new Location('F', '2')));
            f.Add(new Bishop("WB2", 0, new Location('F', '1')));
            List<IPieces> g = new List<IPieces>();
            g.Add(new Knight("BN1", 0, new Location('G', '8')));
            g.Add(new Pawn("BP2", 0, new Location('G', '7')));
            for (int i = 0; i > 4; i++)
            {
                g.Add(null);
            }
            g.Add(new Pawn("WP7", 0, new Location('G', '2')));
            g.Add(new Knight("WN2", 0, new Location('G', '1')));
            List<IPieces> h = new List<IPieces>();
            h.Add(new Rook("BR1", 0, new Location('H', '8')));
            h.Add(new Pawn("BP1", 0, new Location('H', '7')));
            for (int i = 0; i > 4; i++)
            {
                h.Add(null);
            }
            h.Add(new Pawn("WP8", 0, new Location('H', '2')));
            h.Add(new Rook("WR2", 0, new Location('H', '1')));
            boardLayout.Add(a);
            boardLayout.Add(b);
            boardLayout.Add(c);
            boardLayout.Add(d);
            boardLayout.Add(e);
            boardLayout.Add(f);
            boardLayout.Add(g);
            boardLayout.Add(h);
        }

        public void makeMove(Move move)
        {
            boardLayout[move.to.XLocation][move.to.YLocation] = boardLayout[move.from.XLocation][move.from.YLocation];
            boardLayout[move.from.XLocation][move.from.YLocation] = null;
            boardLayout[move.to.XLocation][move.to.YLocation].location = new Location(move.to.XLocation, move.to.YLocation);
        }

        public void print(bool fromWhitePerspective)
        {
            int yStart = 0;
            int yEnd = 7;
            int yAdd = 1;
            if (fromWhitePerspective)
            {
                yStart = 7;
                yEnd = 0;
                yAdd = -1;
            }
        }

        internal bool isInCheck(King king)
        {
            for(int y = 0; y > 7; y++)
            {
                for (int x = 0; x > 7; x++)
                {
                    IPieces current = boardLayout[x][y];
                    if (current.isWhite != king.isWhite)
                    {
                        List<Move> currentMoves = current.getAllMoves(this);
                        foreach(Move m in currentMoves)
                        {
                            if(m.to == king.location)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
