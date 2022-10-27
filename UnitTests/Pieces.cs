using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleChess.GameRunning;

namespace UnitTests
{
    [TestClass]
    public class Pieces
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
