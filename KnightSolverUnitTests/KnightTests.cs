using NUnit.Framework;
using Moq;

using System;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class KnightTests
    {
        private Mock<IBoard> _boardMock = null;

        private static readonly object[] KnightMoveTestCases =
        {
            new object[] { new BoardLocation(2, 1) },
            new object[] { new BoardLocation(4, 1) },
            new object[] { new BoardLocation(1, 2) },
            new object[] { new BoardLocation(5, 2) },
            new object[] { new BoardLocation(1, 4) },
            new object[] { new BoardLocation(5, 4) },
            new object[] { new BoardLocation(2, 5) },
            new object[] { new BoardLocation(4, 5) }
        };

        private static readonly object[] IllegalKnightMoveTestCases =
{
            new object[] { new BoardLocation(3, 1) },
            new object[] { new BoardLocation(5, 1) },
            new object[] { new BoardLocation(2, 2) },
            new object[] { new BoardLocation(4, 2) },
            new object[] { new BoardLocation(3, 4) },
            new object[] { new BoardLocation(5, 5) },
            new object[] { new BoardLocation(1, 5) },
            new object[] { new BoardLocation(3, 5) }
        };

        private static readonly object[] OutOfRangeTestCases =
{
            new object[] { new BoardLocation(0, 1) },
            new object[] { new BoardLocation(1, 0) },
            new object[] { new BoardLocation(6, 1) },
            new object[] { new BoardLocation(1, 6) }
        };

        [SetUp]
        public void Setup()
        {
            _boardMock = new Mock<IBoard>(MockBehavior.Strict);
        }

        [TestCaseSource("KnightMoveTestCases")]
        public void Knight_ValidMoves_FullBoard_Success(IBoardLocation boardLocation)
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3,3));
            
            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);

            knight.Initialize(_boardMock.Object);

            Assert.AreEqual(knight.ValidMoves.Count, 8);
            Assert.IsNotNull(knight.ValidMoves.Find(l => l.X == boardLocation.X && l.Y == boardLocation.Y), 
                $"Valid move ({boardLocation.X},{boardLocation.Y}) is missing");
        }

        [Test]
        public void Knight_ValidMoves_EmptyBoard_Success()
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));

            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);

            knight.Initialize(_boardMock.Object);

            Assert.AreEqual(knight.ValidMoves.Count, 0);
        }

        [Test]
        public void Knight_Initialize_Success()
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));

            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);

            knight.Initialize(_boardMock.Object);

            Assert.AreEqual(knight.BoardLocation.X, 3, $"X Location ({knight.BoardLocation.X}) does not equal expected");
            Assert.AreEqual(knight.BoardLocation.Y, 3, $"Y Location ({knight.BoardLocation.Y}) does not equal expected");
        }

        [Test]
        public void Knight_Initialize_Null()
        {
            Knight knight = new Knight();
                               
            Assert.Throws<NullReferenceException>(() => knight.Initialize(null), $"Failed to throw Exception for null board reference");
        }

        [TestCaseSource("KnightMoveTestCases")]
        public void Knight_Move_Success(IBoardLocation boardLocation)
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));

            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);

            knight.Initialize(_boardMock.Object);

            IMove move = knight.Move(boardLocation);
             
            Assert.IsTrue(move.StartingLocation.Equals(new BoardLocation(3,3)));
            Assert.IsTrue(move.EndingLocation.Equals(boardLocation));
        }

        [TestCaseSource("IllegalKnightMoveTestCases")]
        public void Knight_Move_InvalidSquare_Success(IBoardLocation boardLocation)
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));

            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(true);

            knight.Initialize(_boardMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => knight.Move(boardLocation));
        }

        [TestCaseSource("OutOfRangeTestCases")]
        public void Knight_Move_OutOfRange(IBoardLocation boardLocation)
        {
            Knight knight = new Knight();
            _boardMock.SetupGet(b => b.Width).Returns(5);
            _boardMock.SetupGet(b => b.Height).Returns(5);
            _boardMock.SetupGet(b => b.StartingLocation).Returns(new BoardLocation(3, 3));

            _boardMock.Setup(b => b.IsValidTargetSquare(It.IsAny<BoardLocation>())).Returns(false);

            knight.Initialize(_boardMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => knight.Move(boardLocation),
                $"Failed to throw Exception for out of range starting location ({boardLocation.X},{boardLocation.Y})");
        }
    }
}