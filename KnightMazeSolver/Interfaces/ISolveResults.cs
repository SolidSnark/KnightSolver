using System.Collections.Generic;

namespace KnightMazeSolver
{
    interface ISolveResults
    {
        SquareColor[,] Board { get; }
        List<List<IMove>> Solutions { get; }
        int[] NeighborCount { get; }
        int MovesEvaluated { get; }
        int LongestChainEvaluated { get; }
    }
}