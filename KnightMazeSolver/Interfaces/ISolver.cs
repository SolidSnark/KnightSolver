using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnightMazeSolver
{
    public interface ISolver
    {
        List<List<IMove>> Solve(string filename, SolveType solveType);
        List<List<IMove>> Solve(IBoard board, SolveType solveType);
        List<List<IMove>> Solve(string[] rows, SolveType solveType);
    }
}