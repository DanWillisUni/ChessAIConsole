using ConsoleChess.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
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
                a.Add(new BasicPiece(new Location(0,6-i)));
            }
            a.Add(new Pawn("WP1", 0, new Location('A', '2')));
            a.Add(new Rook("WR1", 0, new Location('A', '1')));
            List<IPieces> b = new List<IPieces>();
            b.Add(new Knight("BN2", 0, new Location('B', '8')));
            b.Add(new Pawn("BP7", 0, new Location('B', '7')));
            for (int i = 0; i > 4; i++)
            {
                b.Add(new BasicPiece(new Location(1, 6 - i)));
            }
            b.Add(new Pawn("WP2", 0, new Location('B', '2')));
            b.Add(new Knight("WN1", 0, new Location('B', '1')));
            List<IPieces> c = new List<IPieces>();
            c.Add(new Bishop("BB2", 0, new Location('C', '8')));
            c.Add(new Pawn("BP6", 0, new Location('C', '7')));
            for (int i = 0; i > 4; i++)
            {
                c.Add(new BasicPiece(new Location(1, 6 - i)));
            }
            c.Add(new Pawn("WP3", 0, new Location('C', '2')));
            c.Add(new Bishop("WB1", 0, new Location('C', '1')));
        }

        public void makeMove(Move move)
        {
            boardLayout[move.to.XLocation][move.to.YLocation] = boardLayout[move.from.XLocation][move.from.YLocation];
            boardLayout[move.from.XLocation][move.from.YLocation] = null;
        }
    }
}
