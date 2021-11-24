using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class SolverTests
    {
        private Mock<IFileSystem> _fileSystemMock = null;

        [SetUp]
        public void Setup()
        {
            _fileSystemMock = new Mock<IFileSystem>();    
        }

        [Test]
        public void Solver_SolveFile_Success()
        {
            ISolver solver = new Solver();
            
            List<List<IMove>> solutions = solver.Solve(@".\TestMazes\SimpleTestMaze.txt");
            Assert.IsTrue(solutions.Count == 1, "Unexpected number of solutions");
            Assert.IsTrue(solutions[0].Count == 2, "Solution has unexpected number of moves");
        }

        [Test]
        public void Solver_SolveFile_FileDoesNotExist()
        {
            _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(false);

            ISolver solver = new Solver();
            Assert.Throws<Exception>(() => solver.Solve(@"F:\\Does\Not\Exist"), "Failed to throw an exception on non-existant file");
        }
        
        [Test]
        public void Solver_SolveFile_ReadAllLinesThrows()
        {
            _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(f => f.File.ReadAllLines(It.IsAny<string>())).Throws(new Exception());

            ISolver solver = new Solver();
            Assert.Throws<Exception>(() => solver.Solve(@"F:\\TestFile"), "Failed to throw an exception on read error");
        }

        [Test]
        public void Solver_Solve_Success()
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
            //board.LoadStateData(rows);
            List<List<IMove>> solutions = solver.Solve(rows);
            int shortestSolution = int.MaxValue;
            foreach (List<IMove> solution in solutions)
            {
                if (solution.Count < shortestSolution)
                {
                    shortestSolution = solution.Count;
                }
            }

            int y = 5;
        }

    }
}