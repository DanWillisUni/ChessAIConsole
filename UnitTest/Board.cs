using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;
using ConsoleChesss;

namespace UnitTest
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void DeepCopyTest()
        {
            Board a = new Board();
            Board b = a.DeepCopy();
            a.makeMove(new Move("E2:E4"));
            Assert.AreEqual("rnbqkbnrpppppppp                    P           PPPP PPPRNBQKBNR", a.printAsString());
            Assert.AreEqual("rnbqkbnrpppppppp                                PPPPPPPPRNBQKBNR", b.printAsString());
        }

        [TestMethod]
        public void DeepCopyTest2()
        {
            Board a = new Board();
            Board b = a.DeepCopy();
            b.makeMove(new Move("E2:E4"));
            Assert.AreEqual("rnbqkbnrpppppppp                    P           PPPP PPPRNBQKBNR", b.printAsString());
            Assert.AreEqual("rnbqkbnrpppppppp                                PPPPPPPPRNBQKBNR", a.printAsString());
        }

        #region onBoard
        [TestMethod]
        public void isOnBoardSuccess()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    bool result = Board.isOnBoard(x, y);
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
                    bool result = Board.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }

                for (int y = 8; y < 10; y++)
                {
                    bool result = Board.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }
            }

            for (int x = 8; x < 10; x++)
            {
                for (int y = -3; y < -1; y++)
                {
                    bool result = Board.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }

                for (int y = 8; y < 10; y++)
                {
                    bool result = Board.isOnBoard(x, y);
                    Assert.AreEqual(result, false);
                }
            }

        }
        #endregion
    }
}
