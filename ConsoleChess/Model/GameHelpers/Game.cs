using System;
using System.Collections.Generic;
using System.Text;
using ConsoleChess.Model.BoardHelpers;

namespace ConsoleChess.Model.GameHelpers
{
    public class Game
    {
        Board boardState { get; set; }
        Player White { get; set; }
        Player Black { get; set; }
        List<Move> allMoves { get; set; }

    }
}
