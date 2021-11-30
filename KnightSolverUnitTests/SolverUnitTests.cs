using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class SolverTests
    {
        private Mock<IFileSystem> _fileSystemMock = null;

        public static IEnumerable<TestCaseData> NotInitializedTestCases
        {
            get
            {
                yield return new TestCaseData(5, 0);
                yield return new TestCaseData(0, 5);
            }
        }

        [SetUp]
        public void Setup()
        {
            _fileSystemMock = new Mock<IFileSystem>();    
        }

        [Test]
        public void Solver_SolveFile_Success()
        {
            ISolver solver = new Solver();
            
            List<List<IMove>> solutions = solver.Solve(@".\TestMazes\SimpleTestMaze.txt", SolveType.Full);
            Assert.That(solutions.Count, Is.EqualTo(1), "Unexpected number of solutions");
            Assert.That(solutions[0].Count, Is.EqualTo(2), "Solution has unexpected number of moves");
        }

        [Test]
        public void Solver_SolveFile_FileDoesNotExist()
        {
            _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(false);

            ISolver solver = new Solver();
            Assert.Throws<Exception>(() => solver.Solve(@"F:\\Does\Not\Exist", SolveType.Full), "Failed to throw an exception on non-existant file");
        }

        [Test]
        public void Solver_SolveFile_ReadAllLinesThrows()
        {
            _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(f => f.File.ReadAllLines(It.IsAny<string>())).Throws(new Exception());

            ISolver solver = new Solver();
            Assert.Throws<Exception>(() => solver.Solve(@"F:\\TestFile", SolveType.Full), "Failed to throw an exception on read error");
        }

        [Test]
        public void Solver_Solve_FindFirst()
        {
            ISolver solver = new Solver();

            string[] rows = new string[] {
                "EXXXXXXXXXX.XX.X....X....X.........X...X..",
                ".............X.X.X.....X.........X...X...X",
                ".XXXXXXXXXXXX....X...X...XX..X.X..X.X...X.",
                "................X..........X.X.X........X.",
                ".XX.X.X.X..XX...X..XX..X...X......X.X.X...",
                "............X....XX..X.....X...XX.........",
                "XXX.XX.XXX.XX..........XX..X.X..X..X...XXX",
                "...............X........XXX..X..X..X...X..",
                "XXXXXXXXXXX.X.....X.XX...........X...X.XX.",
                "................X.X....X.........X...X..X.",
                "XX.XX.XXXXXXX...X.X..X...X.X.X............",
                "X...........XXXX...X.X...X.X.X.X.XX..XXX..",
                "XX.XXXXXXXX.X..X...X...X......XX...XXX.X.S"
            };
            
            List<List<IMove>> solutions = solver.Solve(rows, SolveType.First);

            Assert.That(solutions.Count, Is.EqualTo(1), "Unexpected number of solutions");
            Assert.That(solutions[0].Count, Is.EqualTo(59), "Solution has unexpected number of moves");
        }

        [Test]
        public void Solver_Solve_FindShortest()
        {
            ISolver solver = new Solver();

            string[] rows = new string[] {
                "EXXXXXXXXXX.XX.X....X....X.........X...X..",
                ".............X.X.X.....X.........X...X...X",
                ".XXXXXXXXXXXX....X...X...XX..X.X..X.X...X.",
                "................X..........X.X.X........X.",
                ".XX.X.X.X..XX...X..XX..X...X......X.X.X...",
                "............X....XX..X.....X...XX.........",
                "XXX.XX.XXX.XX..........XX..X.X..X..X...XXX",
                "...............X........XXX..X..X..X...X..",
                "XXXXXXXXXXX.X.....X.XX...........X...X.XX.",
                "................X.X....X.........X...X..X.",
                "XX.XX.XXXXXXX...X.X..X...X.X.X............",
                "X...........XXXX...X.X...X.X.X.X.XX..XXX..",
                "XX.XXXXXXXX.X..X...X...X......XX...XXX.X.S"
            };

            List<List<IMove>> solutions = solver.Solve(rows, SolveType.Shortest);

            Assert.That(solutions.Count, Is.EqualTo(1), "Unexpected number of solutions");
            Assert.That(solutions[0].Count, Is.EqualTo(41), "Solution has unexpected number of moves");
        }

        [TestCaseSource(nameof(NotInitializedTestCases))]
        public void Solver_Solve_UnitializedBoaard(int width, int height)
        {
            ISolver solver = new Solver();
            IKnight knight = new Knight();
            IBoard board = new Board(knight);
            ((Board)board).Width = (byte)width;
            ((Board)board).Height = (byte)height;

            Assert.Throws<InvalidDataException>(() => solver.Solve(board, SolveType.Full), "Failed to throw an exception on uninitilized board");
        }

        [Test]
        public void Solver_Solve_NoStartingPostition()
        {
            ISolver solver = new Solver();
            IKnight knight = new Knight();
            IBoard board = new Board(knight);
            ((Board)board).Width = 5;
            ((Board)board).Height = 5;
            ((Board)board)._boardData = new SquareColor[5, 5];

            board.EndingLocation = new BoardLocation(3, 3);

            Assert.Throws<InvalidDataException>(() => solver.Solve(board, SolveType.Full), "Failed to throw an exception on missing start position");
        }

        [Test]
        public void Solver_Solve_NoEndingPostition()
        {
            ISolver solver = new Solver();
            IKnight knight = new Knight();
            IBoard board = new Board(knight);
            ((Board)board).Width = 5;
            ((Board)board).Height = 5;
            ((Board)board)._boardData = new SquareColor[5, 5];

            board.StartingLocation = new BoardLocation(3, 3);

            Assert.Throws<InvalidDataException>(() => solver.Solve(board, SolveType.Full), "Failed to throw an exception on missing end position");
        }        
    }
}