using ConsoleChess.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Model.BoardHelpers
{
    public class Board
    {
        List<List<IPieces>> boardLayout { get; set; }

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

        }

        public void makeMove(Move move)
        {
            boardLayout[move.to.XLocation][move.to.YLocation] = boardLayout[move.from.XLocation][move.from.YLocation];
            boardLayout[move.from.XLocation][move.from.YLocation] = null;
        }
    }
}
