﻿using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ConsoleChess.AI.Model
{
    public class MoveResult
    {
        public MoveResult(Move originalMove, double lowestScore, Board board)
        {
            this.originalMove = originalMove;
            this.score = lowestScore;
            this.boardAfterMove = board;
        }

        public Move originalMove { get; set; }
        public double score { get; set; }
        public Board boardAfterMove { get; set; }

        public override string ToString() 
        {
            string pastMovesStr = string.Empty;
            bool passedMove = false;
            foreach(Move m in boardAfterMove.pastMoves)
            {
                if (passedMove)
                {
                    pastMovesStr += m.ToString() + ", ";
                }
                else
                {
                    if (m.ToString() == originalMove.ToString())
                    {
                        pastMovesStr += m.ToString() + ", ";
                        passedMove = true;
                    }
                }
            }
            return "Move: " + originalMove.ToString() + " Board: " + boardAfterMove.printAsString() + " Moves: (" + boardAfterMove.pastMoves.Count + ") [" + pastMovesStr + "] - Score: " + score; 
        }
    }
}
