using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    public class Pawn : BasicPiece , IPieces
    {
        public char moveType { get; set; }

        public Pawn(string id, int numberOfMoves, Location location) : base(id, numberOfMoves, location)
        {
            moveType = 'P';
        }


        public List<Move> getPossibleMoves(Board b)
        {
            switch (moveType)
            {
                case 'P':
                    return getPossibleMovesPawn(this, b);
                case 'R':
                    return getPossibleMovesRook(this, b);
                case 'N':
                    return getPossibleMovesKnight(this, b);
                case 'B':
                    return getPossibleMovesBishop(this, b);
                case 'Q':
                    return getPossibleMovesQueen(this, b);
                default:
                    throw new Exception("Not recognised move type");
            }

            
        }
    }
}
