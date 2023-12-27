using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.AI.Variations
{
    public class Scoring
    {
        public static readonly Dictionary<Char, Int16> peiceValues = new Dictionary<Char, Int16>() { { 'P', 1 }, { 'B', 3 }, { 'N', 3 }, { 'R', 5 }, { 'Q', 9 }, { 'K', 200 } };
        public static int getPeiceValue(IPieces p)
        {
            char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
            return peiceValues[type];
        }

        public static double getValueOfThreatening(Board b, List<Move> moves)
        {
            double r = 0;
            foreach (Move move in moves)
            {
                IPieces taken = b.allPeices.Where(o => o.location.Equals(move.toLocation) && o.id.ToUpper()[1] != 'K').Select(o => o).FirstOrDefault();
                if (taken != null)
                {
                    r += Scoring.getPeiceValue(taken);
                }
            }
            return r;
        }

        #region peice square value

        private static double[,] pawnEval = new double[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 5, 10, 10, -20, -20, 10, 10, 5 },
            { 5, -5, -10, 0, 0, -10, -5, 5 },
            { 0, 0, 0, 20, 20, 0, 0, 0 },
            { 5, 5, 10, 25, 25, 10, 5, 5 },
            { 10, 10, 20, 30, 30, 20, 10, 10 },
            { 50, 50, 50, 50, 50, 50, 50, 50 },
            { 0, 0, 0, 0, 0, 0, 0, 0 } 
        };

        private static double[,] knightEval = new double[,] {
            { -50, -40, -30, -30, -30, -30, -40, -50, },
            { -40, -20, 0, 0, 0, 0, -20, -40, },
            { -30, 0, 10, 15, 15, 10, 0, -30 },
            { -30, 5, 15, 20, 20, 15, 5, -30 },
            { -30, 0, 15, 20, 20, 15, 0, -30 },
            { -30, 5, 10, 15, 15, 10, 5, -30 },
            { -40, -20, 0, 5, 5, 0, -20, -40 },
            { -50, -40, -30, -30, -30, -30, -40, -50 }
        };

        private static double[,] bishopEval = new double[,] {
            { -20, -10, -10, -10, -10, -10, -10, -20 },
            { -10, 5, 0, 0, 0, 0, 5, -10 },
            { -10, 10, 10, 10, 10, 10, 10, -10 },
            { -10, 0, 10, 10, 10, 10, 0, -10 },
            { -10, 5, 5, 10, 10, 5, 5, -10 },
            { -10, 0, 5, 10, 10, 5, 0, -10 },
            { -10, 0, 0, 0, 0, 0, 0, -10 },
            { -20, -10, -10, -10, -10, -10, -10, -20 },
        };

        private static double[,] rookEval = new double[,] {
            { 0, 0, 0, 5, 5, 0, 0, 0 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { 5, 10, 10, 10, 10, 10, 10, 5 },
            { 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        private static double[,] queenEval = new double[,] {
            { -20, -10, -10, -5, -5, -10, -10, -20 },
            { -10, 0, 0, 0, 0, 0, 0, -10 },
            { -10, 0, 5, 5, 5, 5, 0, -10 },
            { -5, 0, 5, 5, 5, 5, 0, -5 },
            { 0, 0, 5, 5, 5, 5, 0, -5 },
            { -10, 5, 5, 5, 5, 5, 0, -10 },
            { -10, 0, 5, 0, 0, 0, 0, -10 },
            { -20, -10, -10, -5, -5, -10, -10, -20 }
        };

        private static double[,] kingEval = new double[,] {
            { 20, 30, 10, 0, 0, 10, 30, 20 },
            { 20, 20, 0, 0, 0, 0, 20, 20 },
            { -10, -20, -20, -20, -20, -20, -20, -10 },
            { 20, -30, -30, -40, -40, -30, -30, -20 },
            { -30, -40, -40, -50, -50, -40, -40, -30 },
            { -30, -40, -40, -50, -50, -40, -40, -30 },
            { -30, -40, -40, -50, -50, -40, -40, -30 },
            { -30, -40, -40, -50, -50, -40, -40, -30 },
        };

        private static double[,] kingEndEval = new double[,] {
            { 50, -30, -30, -30, -30, -30, -30, -50 },
            { -30, -30,  0,  0,  0,  0, -30, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -20, -10,  0,  0, -10, -20, -30 },
            { -50, -40, -30, -20, -20, -30, -40, -50 },
        };

        public static double getPeiceSquareValue(IPieces p, bool isEndGame = false)
        {
            double r = 0;
            char type = p.id[1] == 'P' ? ((Pawn)p).moveType : p.id[1];
            int xCoord = p.isWhite ? 7 - p.location.getXCoord() : p.location.getXCoord();
            int yCoord = p.isWhite ? 7 - p.location.getYCoord() : p.location.getYCoord();
            switch (type)
            {
                case 'P':
                    r = pawnEval[xCoord, yCoord];
                    break;
                case 'N':
                    r = knightEval[xCoord, yCoord];
                    break;
                case 'B':
                    r = bishopEval[xCoord, yCoord];
                    break;
                case 'R':
                    r = rookEval[xCoord, yCoord];
                    break;
                case 'Q':
                    r = queenEval[xCoord, yCoord];
                    break;
                case 'K':
                    r = isEndGame ? kingEndEval[xCoord, yCoord] : kingEval[xCoord, yCoord];
                    break;
            }
            return r * 0.01;
        }
        #endregion
    }
}
