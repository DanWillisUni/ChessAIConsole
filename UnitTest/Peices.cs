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
    }
}
