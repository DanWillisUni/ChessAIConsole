using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.AI.Model
{
    public class MoveResult
    {
        public MoveResult(Move originalMove, int lowestScore, Board board)
        {
            this.originalMove = originalMove;
            this.score = lowestScore;
            this.boardAfterMove = board;
        }

        public Move originalMove { get; set; }
        public int score { get; set; }
        public Board boardAfterMove { get; set; }

        public override string ToString() { return "Move: " + originalMove.ToString() + " - Score: " + score; }
    }
}
