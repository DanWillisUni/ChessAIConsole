﻿using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.Pieces
{
    public class BasicPiece
    {
        public string id { get; set; }
        public bool isWhite { get; set; }
        public int numberOfMoves { get; set; }
        public Location location { get; set; }
        public BasicPiece(string id, int numberOfMoves, Location location)
        {
            this.id = id;
            isWhite = id[0] == 'W' ? true : false;
            this.numberOfMoves = numberOfMoves;
            this.location = location;
        }
        #region peice movement
        public static List<Move> getPossibleMovesPawn(Pawn pieceToMove,Board board)
        {
            //Handle Promotions
            if (pieceToMove.moveType == 'Q')
            {
                return getPossibleMovesQueen(pieceToMove,board);
            }
            else if (pieceToMove.moveType == 'B')
            {
                return getPossibleMovesBishop(pieceToMove, board);
            }
            else if (pieceToMove.moveType == 'R')
            {
                return getPossibleMovesRook(pieceToMove, board);
            }
            else if (pieceToMove.moveType == 'K')
            {
                return getPossibleMovesKnight(pieceToMove, board);
            }

            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            int forwardMultiplyer = (pieceToMove.isWhite ? 1:-1);            

            if (String.IsNullOrEmpty(board.layout[fromXCoord, fromYCoord + forwardMultiplyer]))
            {
                if (pieceToMove.numberOfMoves == 0)
                {
                    if (String.IsNullOrEmpty(board.layout[fromXCoord,fromYCoord + (2*forwardMultiplyer)]))
                    {
                        moves.Add(new Move(pieceToMove.location, new Location(fromXCoord, fromYCoord + (2 * forwardMultiplyer))));
                    }
                }
                moves.Add(new Move(pieceToMove.location, new Location(fromXCoord, fromYCoord + forwardMultiplyer)));
            }



            //still need enpassant

            
            
            return moves;
        }
        public static List<Move> getPossibleMovesRook(IPieces pieceToMove, Board board)
        {
            List<Move> moves = getMovesInDirection(pieceToMove, 1, 0, board);
            foreach(Move newMoves in getMovesInDirection(pieceToMove, 0, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, 0, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, 0, 1, board))
            {
                moves.Add(newMoves);
            }
            return moves;
        }
        public static List<Move> getPossibleMovesKnight(IPieces pieceToMove, Board board)
        {
            //throw new NotImplementedException();
            return new List<Move>();
        }
        public static List<Move> getPossibleMovesBishop(IPieces pieceToMove, Board board)
        {
            List<Move> moves = getMovesInDirection(pieceToMove, 1, 1, board);
            foreach (Move newMoves in getMovesInDirection(pieceToMove, 1, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, -1, board))
            {
                moves.Add(newMoves);
            }
            foreach (Move newMoves in getMovesInDirection(pieceToMove, -1, 1, board))
            {
                moves.Add(newMoves);
            }
            return moves;
        }
        public static List<Move> getPossibleMovesQueen(IPieces pieceToMove, Board board)
        {
            List<Move> all = getPossibleMovesBishop(pieceToMove, board);
            all.AddRange(getPossibleMovesRook(pieceToMove, board));
            return all;
        }
        public static List<Move> getPossibleMovesKing(King pieceToMove, Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();

            for(int x = -1;x > 1; x++)
            {
                for (int y = -1; y > 1; y++)
                {
                    if(!(x == 0 && y == 0))
                    {
                        if (!(fromXCoord + x < 0 || fromXCoord + x > 7 || fromYCoord + y > 7 || fromYCoord + y < 0))
                        {
                            string current = board.layout[fromXCoord + x,fromYCoord + y];
                            if (current == "" || (pieceToMove.isWhite ? current[0] == 'B' : current[0] == 'W'))
                            {
                                if (!board.isInCheck(pieceToMove))
                                {
                                    moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + x, fromYCoord + y)));
                                }
                            }                            
                        }
                    }
                }
            }
            return moves;
            
        }
        private static List<Move> getMovesInDirection(IPieces pieceToMove,int x,int y,Board board)
        {
            List<Move> moves = new List<Move>();
            int fromXCoord = pieceToMove.location.getXCoord();
            int fromYCoord = pieceToMove.location.getYCoord();
            bool canContinue = true;
            int count = 0;
            do
            {
                count++;
                if (fromXCoord + (x * count) < 0|| fromXCoord + (x * count)>7|| fromYCoord + (y * count)>7|| fromYCoord + (y * count)<0)
                {
                    canContinue = false;
                } 
                else
                {
                    string current = board.layout[fromXCoord + (x * count),fromYCoord + (y * count)];
                    if (String.IsNullOrEmpty(current))
                    {
                        moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                    }
                    else
                    { 
                        if (pieceToMove.isWhite ? current[0] == 'B' : current[0] == 'W')
                        {
                            moves.Add(new Move(pieceToMove.location, new Location(fromXCoord + (x * count), fromYCoord + (y * count))));
                        }
                        canContinue = false;
                    }
                }
                
            } while (canContinue);
            return moves;
        }
        #endregion
    }
}
