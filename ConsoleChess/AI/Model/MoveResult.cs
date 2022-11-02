using ConsoleChess.Model.BoardHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChess.AI.Model
{
    public class MoveResult
    {
        public MoveResult(Move m, int lowestScore)
        {
            this.m = m;
            this.score = lowestScore;
        }

        public Move m { get; set; }
        public int score { get; set; }
    }
}
