using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleChess.GameRunning;
using ConsoleChess.Pieces;
using ConsoleChesss;
using ConsoleChess.Model.BoardHelpers;

namespace UnitTest
{
    [TestClass]
    public class PiecesTest
    {
        #region Pawn
        [TestMethod]
        public void Pawn1Forward()
        {
            Board b = new Board();

        }
        [TestMethod]
        public void Pawn2Forward()
        {
        }
        [TestMethod]
        public void PawnTakeForward()
        {
        }
        [TestMethod]
        public void EnPassent()
        {
        }
        [TestMethod]
        public void PawnForwardObstructed()
        {
        }
        #endregion
        #region Bishop
        #endregion
        #region Rook
        #endregion
        #region Knight
        #endregion
        #region King
        [TestMethod]
        public void KingAround()
        {
            Board b = new Board(true);
            b.addPeice("WK1", new Location("E2"));
            List<Move> all = b.getAllMoves(true);
            Assert.AreEqual(8, all.Count);
            foreach (Move move in all)
            {
                Assert.IsNotNull(move);
                Assert.AreEqual(move.fromLocation, new Location("E2"));
            }
        }
        [TestMethod]
        public void KingNotIntoCheck()
        {
        }
        [TestMethod]
        public void CastleLeft()
        {
        }
        [TestMethod]
        public void CastleRight()
        {
        }
        #endregion
        #region onBoard
        [TestMethod]
        public void isOnBoardSuccess()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    bool result = BasicPiece.isOnBoard(x, y);
                    Assert.AreEqual(result, true);
                }
            }

        }

        [TestMethod]
        public void isOnBoardFail()
        {
            for (int x = -3; x < -1; x++)
            {
                for (int y = -3; y < -1; y++)
                {
                    bool result = BasicPiece.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }

                for (int y = 8; y < 10; y++)
                {
                    bool result = BasicPiece.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }
            }

            for (int x = 8; x < 10; x++)
            {
                for (int y = -3; y < -1; y++)
                {
                    bool result = BasicPiece.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }

                for (int y = 8; y < 10; y++)
                {
                    bool result = BasicPiece.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }
            }

        }
        #endregion
    }
}
