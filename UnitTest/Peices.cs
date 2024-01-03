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
        [TestMethod]
        public void PawnPromoteToKnightBlack()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E2");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "F3");
            b.addPeice('P', false, "G2");
            b.addPeice('R', false, "D1");
            b.addPeice('N', false, "E5");
            b.addPeice('B', false, "A4");
            Assert.IsNotNull(b.allPeices);
            Assert.AreEqual("    k                       n   b            r      K p    r    ", b.printAsString());
            b.makeMove("G2:G1");
            Assert.AreEqual("    k                       n   b            r      K      r  n ", b.printAsString());
            Assert.IsTrue(b.isCheckmate(true));
        }
        [TestMethod]
        public void PawnPromoteToQueenWhite()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('K', false, "E7");
            b.addPeice('P', true, "G7");
            Assert.IsNotNull(b.allPeices);
            Assert.AreEqual("            k P                                             K   ", b.printAsString());
            b.makeMove("G7:G8");
            Assert.AreEqual("      Q     k                                               K   ", b.printAsString());
        }

        [TestMethod]
        public void PawnPromoteToQueenBlack()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E2");
            b.addPeice('K', false, "E8");
            b.addPeice('P', false, "G2");
            Assert.IsNotNull(b.allPeices);
            Assert.AreEqual("    k                                               K p         ", b.printAsString());
            b.makeMove("G2:G1");
            Assert.AreEqual("    k                                               K         q ", b.printAsString());
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
        public void CastleKingside()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "H1");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "H8");
            b.addPeice('P', false, "F6");
            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesWhite);
            Assert.IsTrue(possibleMovesWhite.Contains(new Move("E1:G1")));
            Assert.AreEqual(b.printAsString(), "    k  r             p                                      K  R");
            b.makeMove(new Move("E1:G1"));
            Assert.AreEqual(b.printAsString(), "    k  r             p                                       RK ");

            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesBlack);
            Assert.IsTrue(possibleMovesBlack.Contains(new Move("E8:G8")));
            Assert.AreEqual(b.printAsString(), "    k  r             p                                       RK ");
            b.makeMove(new Move("E8:G8"));
            Assert.AreEqual(b.printAsString(), "     rk              p                                       RK ");
        }
        [TestMethod]
        public void CastleQueenside()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "A1");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "A8");
            b.addPeice('P', false, "D6");

            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesWhite);
            Assert.IsTrue(possibleMovesWhite.Contains(new Move("E1:C1")));
            Assert.AreEqual(b.printAsString(), "r   k              p                                    R   K   ");
            b.makeMove(new Move("E1:C1"));
            Assert.AreEqual(b.printAsString(), "r   k              p                                      KR    ");

            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesBlack);
            Assert.IsTrue(possibleMovesBlack.Contains(new Move("E8:C8")));
            Assert.AreEqual(b.printAsString(), "r   k              p                                      KR    ");
            b.makeMove(new Move("E8:C8"));
            Assert.AreEqual(b.printAsString(), "  kr               p                                      KR    ");
        }
        [TestMethod]
        public void CastleKingsideThroughCheckBlock()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "H1");
            b.addPeice('R', true, "F2");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "H8");
            b.addPeice('R', false, "G7");

            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsFalse(possibleMovesWhite.Count == 0);
            Assert.IsTrue(!possibleMovesWhite.Contains(new Move("E1:G1")));
            
            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsFalse(possibleMovesBlack.Count == 0);
            Assert.IsTrue(!possibleMovesBlack.Contains(new Move("E8:G8")));
        }
        [TestMethod]
        public void CastleQueensideThroughCheckBlock()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "A1");
            b.addPeice('R', true, "B2");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "A8");
            b.addPeice('R', false, "D7");

            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesWhite);
            Assert.IsFalse(possibleMovesWhite.Contains(new Move("E1:C1")));

            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesBlack);
            Assert.IsFalse(possibleMovesBlack.Contains(new Move("E8:C8")));
        }
        [TestMethod]
        public void CastleKingsideInCheckBlock()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "H1");
            b.addPeice('R', true, "E6");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "H8");
            b.addPeice('R', false, "E3");

            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesWhite);
            Assert.IsFalse(possibleMovesWhite.Contains(new Move("E1:G1")));

            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesBlack);
            Assert.IsFalse(possibleMovesBlack.Contains(new Move("E8:G8")));
        }
        [TestMethod]
        public void CastleQueensideInCheckBlock()
        {
            Board b = new Board(true);
            b.addPeice('K', true, "E1");
            b.addPeice('R', true, "A1");
            b.addPeice('R', true, "E6");
            b.addPeice('K', false, "E8");
            b.addPeice('R', false, "A8");
            b.addPeice('R', false, "E3");

            Assert.IsNotNull(b.allPeices);
            IPieces whiteKing = b.allPeices.Where(o => o.location.Equals(new Location("E1"))).Select(o => o).FirstOrDefault();
            IPieces blackKing = b.allPeices.Where(o => o.location.Equals(new Location("E8"))).Select(o => o).FirstOrDefault();

            Assert.IsNotNull(whiteKing);
            List<Move> possibleMovesWhite = whiteKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesWhite);
            Assert.IsFalse(possibleMovesWhite.Contains(new Move("E1:C1")));

            Assert.IsNotNull(blackKing);
            List<Move> possibleMovesBlack = blackKing.getPossibleMoves(b);
            Assert.IsNotNull(possibleMovesBlack);
            Assert.IsFalse(possibleMovesBlack.Contains(new Move("E8:C8")));
        }

        #endregion
    }
}
