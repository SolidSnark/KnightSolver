using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;
using System.IO;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{    
    public class BoardTests
    {
        Mock<IKnight> _knightMock = null;

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

        public static IEnumerable<TestCaseData> InRangeTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(1, 1));
                yield return new TestCaseData(new BoardLocation(5, 1));
                yield return new TestCaseData(new BoardLocation(3, 3));
                yield return new TestCaseData(new BoardLocation(1, 5));
                yield return new TestCaseData(new BoardLocation(5, 5));
            } 
        }

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

        private static readonly SquareColor[,] _emptyBoardData = new SquareColor[5, 5]
        {
            { SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void},
            { SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void},
            { SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void},
            { SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void},
            { SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Void}
        };

        private static readonly SquareColor[,] _checkerboardBoardData = new SquareColor[5, 5]
        {
                { SquareColor.White, SquareColor.Black, SquareColor.White, SquareColor.Black, SquareColor.White},
                { SquareColor.Black, SquareColor.White, SquareColor.Black, SquareColor.White, SquareColor.Black},
                { SquareColor.White, SquareColor.Black, SquareColor.White, SquareColor.Black, SquareColor.White},
                { SquareColor.Black, SquareColor.White, SquareColor.Black, SquareColor.White, SquareColor.Black},
                { SquareColor.White, SquareColor.Black, SquareColor.White, SquareColor.Black, SquareColor.White}
        };

        private static readonly SquareColor[,] _knightMovesdBoardData = new SquareColor[5, 5]
        {
            { SquareColor.Void, SquareColor.Black, SquareColor.Void, SquareColor.Black, SquareColor.Void},
            { SquareColor.Black, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Black},
            { SquareColor.Void, SquareColor.Void, SquareColor.White, SquareColor.Void, SquareColor.Void},
            { SquareColor.Black, SquareColor.Void, SquareColor.Void, SquareColor.Void, SquareColor.Black},
            { SquareColor.Void, SquareColor.Black, SquareColor.Void, SquareColor.Black, SquareColor.Void}
        };

        [SetUp]
        public void Setup()
        {
            _knightMock = new Mock<IKnight>(MockBehavior.Strict);
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_StartingLocationSet_Success(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);

            Assert.DoesNotThrow(() => board.StartingLocation = boardLocation, $"Exception setting starting location to ({boardLocation.X},{boardLocation.Y})");
            Assert.AreEqual(board.StartingLocation, boardLocation, $"Setting starting location to {boardLocation.X},{boardLocation.Y} Failed");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_StartingLocationSet_EqualsEndingLocation(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);
            board.EndingLocation = boardLocation;

            Assert.Throws<ArgumentException>(() => board.StartingLocation = boardLocation,
                $"Failed to throw Exception for starting location ({boardLocation.X},{boardLocation.Y}) equal to ending location");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_StartingLocationSet_OutOfRange(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() => board.StartingLocation = boardLocation, 
                $"Failed to throw Exception for out of range starting location ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_EndingLocationSet_Success(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);

            Assert.DoesNotThrow(() => board.EndingLocation = boardLocation, $"Exception setting ending location to ({boardLocation.X},{boardLocation.Y})");
            Assert.AreEqual(board.EndingLocation, boardLocation, $"Setting ending location to {boardLocation.X},{boardLocation.Y} Failed");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_EndingLocationSet_EqualsStartingLocation(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);
            board.StartingLocation = boardLocation;

            Assert.Throws<ArgumentException>(() => board.EndingLocation = boardLocation,
                $"Failed to throw Exception for ending location ({boardLocation.X},{boardLocation.Y}) equal to starting location");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_EndingLocationSet_OutOfRange(IBoardLocation boardLocation)
        {
            IBoard board = CreateBoard(5, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() => board.EndingLocation = boardLocation, 
                    $"Failed to throw Exception for out of range ending location ({boardLocation.X},{boardLocation.Y})");
        }

        [Test]
        public void Board_Initialize_Success()
        {            
            _knightMock.Setup(i => i.Initialize(It.IsAny<Board>()));
            Board board = new Board(_knightMock.Object);

            board.Initialize(5, 5);

            Assert.AreEqual(board.Width, 5, "Width does not equal expected");
            Assert.AreEqual(board.Height, 5, "Height does not equal expected");
            Assert.AreEqual(board._boardData.Length, 5 * 5, "BoardData length does not equal expected");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_Initialize_OutOfRange(IBoardLocation boardLocation)
        {
            _knightMock.Setup(i => i.Initialize(It.IsAny<Board>()));
            Board board = new Board(_knightMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => board.Initialize(boardLocation.X, boardLocation.Y), 
                    $"Failed to throw Exception for out of range intialization ({boardLocation.X}x{boardLocation.Y})");
        }

        [Test]
        public void Board_LoadStateData_Success()
        {
            Board board = new Board(_knightMock.Object);
            IBoardLocation testStartingLocation = new BoardLocation(2, 1);
            IBoardLocation testEndingLocation = new BoardLocation(4, 5);            

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));
            
            string[] rows = new string[] {
                ".S.X.",
                "X...X",
                "..X..",
                "X...X",
                ".X.E."
            };

            board.LoadStateData(rows);

            _knightMock.Verify(k => k.Initialize(It.IsAny<IBoard>()));

            Assert.AreEqual(board.Width, 5, "Width does not equal expected");
            Assert.AreEqual(board.Height, 5, "Height does not equal expected");
            Assert.AreEqual(board._boardData.Length, 5 * 5, "BoardData length does not equal expected");
            Assert.True(board.StartingLocation.Equals(testStartingLocation), "Starting location does not equal expected");
            Assert.True(board.EndingLocation.Equals(testEndingLocation), "Ending location does not equal expected");
            Assert.True(CompareBoards(5, 5, _knightMovesdBoardData, board._boardData), "BoardData does not equal expected");
        }

        [Test]
        public void Board_LoadStateDataCheckerboard_Success()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(i => i.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "XSXXX",
                "XXXXX",
                "XXXXX",
                "XXXXX",
                "XXXEX"
            };

            board.LoadStateData(rows);

            Assert.True(CompareBoards(5, 5, _checkerboardBoardData, board._boardData), "BoardData does not equal expected");
        }

        [Test]
        public void Board_LoadStateData_ExceedsMaximum_HeightOutOfRange()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                ".S.X.","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",
                "XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX","XXXXX",".X.E."
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Exceeding maximum Height failed to cause Exeception");
        }


        [Test]
        public void Board_LoadStateData_FailsToMeetMinimum_HeightOutOfRange()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                ".S.X.",
                "XXXXX",
                "XXXXX",
                ".X.E."
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Failing to meet minimum Height failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_ExceedsMaximum_WidthOutOfRange()
        {
            Board board = new Board(_knightMock.Object);
            IBoardLocation testStartingLocation = new BoardLocation(2, 1);
            IBoardLocation testEndingLocation = new BoardLocation(4, 5);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                ".S.XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                "XXXX",
                "XXXX",
                "XXXX",
                ".X.E"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Exceeding maximum Width failed to cause Exeception");

        }

        [Test]
        public void Board_LoadStateData_FailsToMeetMinimum_WidthOutOfRange()
        {
            Board board = new Board(_knightMock.Object);
            IBoardLocation testStartingLocation = new BoardLocation(2, 1);
            IBoardLocation testEndingLocation = new BoardLocation(4, 5);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                ".S.X",
                "XXXX",
                "XXXX",
                "XXXX",
                ".X.E"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Failing to meet minimum Width failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_InconsistentLineLength()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "X.S.X",
                "XXXXX",
                "XXXXX",
                "XXXXXX",
                ".X.EX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Inconsistent line length failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_InvalidCharacter()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "XSX.X",
                "XXXXX",
                "XX?XX",
                "XXXXS",
                ".X.EX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Invalid character in board data failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_NoStartingPostion()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "X.X.X",
                "XXXXX",
                "XXXXX",
                "XXXXX",
                ".X.EX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "No starting postition in board data failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_TooManyStartingPostions()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "XSX.X",
                "XXXXX",
                "XXXXX",
                "XXXXS",
                ".X.EX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Too many starting postitions in board data failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_NoEndingPostion()
        {
            Board board = new Board(_knightMock.Object);

            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "X.X.X",
                "XXXXX",
                "XXXXX",
                "XXXXX",
                ".X.SX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "No ending postition in board data failed to cause Exeception");
        }

        [Test]
        public void Board_LoadStateData_TooManyEndingPostions()
        {
            Board board = new Board(_knightMock.Object);
            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            string[] rows = new string[] {
                "XSX.X",
                "XXEXX",
                "XXXXX",
                "XXXXX",
                ".X.EX"
            };

            Assert.Throws<InvalidDataException>(() => board.LoadStateData(rows), "Too many ending postitions in board data failed to cause Exeception");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_SetSquareStateWhite_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _emptyBoardData);
            
            board[boardLocation] = SquareColor.White;
            Assert.AreEqual(board._boardData[boardLocation.X - 1, boardLocation.Y - 1], SquareColor.White,
                $"Failed to set square at ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_SetSquareStateBlack_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _emptyBoardData);

            board[boardLocation] = SquareColor.Black;
            Assert.AreEqual(board._boardData[boardLocation.X - 1, boardLocation.Y - 1], SquareColor.Black,
                $"Failed to set square at ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_SetSquareStateVoid_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _checkerboardBoardData);

            board[boardLocation] = SquareColor.White;
            Assert.AreEqual(board._boardData[boardLocation.X - 1, boardLocation.Y - 1], SquareColor.White,
                $"Failed to set square at ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_SetSquareState_OutOfRange(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _checkerboardBoardData);

            Assert.Throws<ArgumentOutOfRangeException>(() => board[boardLocation] = SquareColor.White,
                $"Failed to throw Exception for out of range location ({boardLocation.X},{boardLocation.Y})");
        }

        [Test]
        public void Board_GetSquareData_Success()
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.AreEqual(board[1, 1], SquareColor.Void, "BoardData does not equal expected (SquareColor.Void)");
            Assert.AreEqual(board[4, 5], SquareColor.Black, "BoardData does not equal expected (SquareColor.Black)");
            Assert.AreEqual(board[3, 3], SquareColor.White, "BoardData does not equal expected (SquareColor.White)");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_GetSquareState_OutOfRange(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            SquareColor sc = SquareColor.Void;
            Assert.Throws<ArgumentOutOfRangeException>(() => sc = board[boardLocation],
                $"Failed to throw Exception for out of range location ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(InRangeTestCases))]
        public void Board_IsSquareInBounds_True_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsTrue(board.IsSquareInBounds(boardLocation), $"Expected true at location ({boardLocation.X},{boardLocation.Y})");
        }

        [Test]
        public void Board_IsSquareInBounds_Null()
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsFalse(board.IsSquareInBounds(null), $"Expected false on null reference");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_IsSquareInBounds_OutOfRange(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsFalse(board.IsSquareInBounds(boardLocation), $"Expected true at location ({boardLocation.X},{boardLocation.Y})");
        }

        [Test]
        public void Board_IsSquareInBounds_OutOfRangeWidthZero()
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsFalse(board.IsSquareInBounds(new BoardLocation(0, 1)), $"Expected false at location (0,1)");
        }

        [Test]
        public void Board_IsSquareInBounds_OutOfRangeHeightZero()
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsFalse(board.IsSquareInBounds(new BoardLocation(1, 0)), $"Expected false at location (1,0)");
        }

        [TestCaseSource(nameof(KnightMoveTestCases))]
        public void Board_IsTargetValidSquare_True_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsTrue(board.IsValidTargetSquare(boardLocation), $"Expected true at location ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(KnightMoveTestCases))]
        public void Board_IsValidTargetSquare_False_Success(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _emptyBoardData);

            Assert.IsFalse(board.IsValidTargetSquare(boardLocation), $"Expected false at location ({boardLocation.X},{boardLocation.Y})");
        }

        [TestCaseSource(nameof(OutOfRangeTestCases))]
        public void Board_IsValidTargetSquare_OutOfRange(IBoardLocation boardLocation)
        {
            Board board = (Board)CreateBoard(5, 5, _knightMovesdBoardData);

            Assert.IsFalse(board.IsValidTargetSquare(boardLocation),
                $"Failed to throw Exception for out of range starting location ({boardLocation.X},{boardLocation.Y})");
        }

        private IBoard CreateBoard(byte width, byte height, SquareColor[,] boardData = null)
        {
            Board board = new Board(_knightMock.Object);
            _knightMock.Setup(k => k.Initialize(It.IsAny<Board>()));

            board.Height = height;
            board.Width = width;            
            board._boardData = boardData != null ? boardData : new SquareColor[width, height];

            return board;
        }

        private bool CompareBoards(byte width, byte height, SquareColor[,] boardOne, SquareColor[,] boardTwo)
        {
            for (byte y = 0; y < height; y++)
            {
                for (byte x = 0; x < width; x++)
                {
                    if (boardOne[x, y] != boardTwo[x, y])
                    {
                        return false;
                    }    
                }
            }

            return true;
        }
    }
}
