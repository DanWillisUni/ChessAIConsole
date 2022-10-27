using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleChess.GameRunning.Player
{
    public class BasicPlayer
    {
        public List<Move> getAllMoves(bool isWhite, Board b)
        {
            List<IPieces> pieces = (from p in b.allPeices
                            where p.isWhite == isWhite
                            select p).ToList();
            List<Move> all = new List<Move>();
            foreach (IPieces p in pieces)
            {
                all.AddRange(p.getPossibleMoves(b));
            }            
            return all;
        }
    }
}
