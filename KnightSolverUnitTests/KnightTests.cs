using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class KnightTests
    {
        private Mock<IBoard> _boardMock = null;

        public static IEnumerable<TestCaseData> KnightMoveTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(2, 1));
                yield return new TestCaseData(new BoardLocation(4, 1));
                yield return new TestCaseData(new BoardLocation(1, 2));
                yield return new TestCaseData(new BoardLocation(5, 2));
                yield return new TestCaseData(new BoardLocation(1, 4));
                yield return new TestCaseData(new BoardLocation(5, 4));
                yield return new TestCaseData(new BoardLocation(2, 5));
                yield return new TestCaseData(new BoardLocation(4, 5));
            }
        }

        public static IEnumerable<TestCaseData> IllegalKnightMoveTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(3, 1));
                yield return new TestCaseData(new BoardLocation(5, 1));
                yield return new TestCaseData(new BoardLocation(2, 2));
                yield return new TestCaseData(new BoardLocation(4, 2));
                yield return new TestCaseData(new BoardLocation(3, 4));
                yield return new TestCaseData(new BoardLocation(5, 5));
                yield return new TestCaseData(new BoardLocation(1, 5));
                yield return new TestCaseData(new BoardLocation(3, 5));
            }
        }

        public static IEnumerable<TestCaseData> OutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(0, 1));
                yield return new TestCaseData(new BoardLocation(1, 0));
                yield return new TestCaseData(new BoardLocation(6, 1));
                yield return new TestCaseData(new BoardLocation(1, 6));
            }
        }

        [SetUp]
        public void Setup()
        {
            _boardMock = new Mock<IBoard>(MockBehavior.Strict);

            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));
        }

        [TestCaseSource(nameof(KnightMoveTestCases))]
        public void Knight_ValidMoves_FullBoard_Success(IBoardLocation boardLocation)
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);

            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            Assert.That(knight.ValidMoves.Count, Is.EqualTo(8), "HACK HACK");
            Assert.IsNotNull(knight.ValidMoves.Find(l => l.X == boardLocation.X && l.Y == boardLocation.Y), 
                $"Valid move ({boardLocation.X},{boardLocation.Y}) is missing");
        }

        [Test]
        public void Knight_ValidMoves_EmptyBoard_Success()
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);

            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            Assert.That(knight.ValidMoves.Count, Is.EqualTo(0));
        }

        [Test]
        public void Knight_Initialize_Success()
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);
            
            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            Assert.That(knight.BoardLocation.X, Is.EqualTo(3), $"X Location ({knight.BoardLocation.X}) does not equal expected");
            Assert.That(knight.BoardLocation.Y, Is.EqualTo(3), $"Y Location ({knight.BoardLocation.Y}) does not equal expected");
        }

        [Test]
        public void Knight_Initialize_Null()
        {
            Knight knight = new Knight();
                               
            Assert.Throws<NullReferenceException>(() => knight.Initialize(null), $"Failed to throw Exception for null board reference");
        }

        [TestCaseSource(nameof(KnightMoveTestCases))]
        public void Knight_Move_Success(IBoardLocation boardLocation)
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);
            
            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            IMove move = knight.Move(boardLocation);
             
            Assert.IsTrue(move.StartingLocation.Equals(new BoardLocation(3,3)));
            Assert.IsTrue(move.EndingLocation.Equals(boardLocation));
        }

        [TestCaseSource(nameof(IllegalKnightMoveTestCases))]
        public void Knight_Move_InvalidSquare_Success(IBoardLocation boardLocation)
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);

            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => knight.Move(boardLocation));
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Knight_Move_OutOfRange(IBoardLocation boardLocation)
        {
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);

            Knight knight = new Knight();
            knight.Initialize(_boardMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => knight.Move(boardLocation),
                $"Failed to throw Exception for out of range starting location ({boardLocation.X},{boardLocation.Y})");
        }
    }
}