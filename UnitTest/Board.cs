using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleChess.GameRunning;
using ConsoleChess.Model.BoardHelpers;

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
    }
}
