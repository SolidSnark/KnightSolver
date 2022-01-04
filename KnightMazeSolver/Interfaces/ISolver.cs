using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnightMazeSolver
{
    public interface ISolver
    {
        ISolveResults Solve(string filename, SolveType solveType);
        ISolveResults Solve(IBoard board, SolveType solveType);
        ISolveResults Solve(string[] rows, SolveType solveType);
    }
}