using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.AI.Model
{
    public class MoveResult
    {
        public MoveResult(Move originalMove, Move targetMove, int lowestScore, Board board)
        {
            if (originalMove == null)
            {
                this.originalMove = targetMove;
            }
            else
            {
                this.originalMove = originalMove;
            }
            this.targetMove = targetMove;
            this.score = lowestScore;
            this.boardAfterMove = board;
        }

        public Move originalMove { get; set; }
        public Move targetMove { get; set; }
        public int score { get; set; }
        public Board boardAfterMove { get; set; }

        public override string ToString() { return "Move: " + originalMove.ToString() + " - Score: " + score; }
    }
}
