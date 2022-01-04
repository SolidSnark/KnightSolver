using System.Collections.Generic;

namespace KnightMazeSolver
{
    public interface ISolveResults
    {
        IBoard Board { get; }
        List<List<IMove>> Solutions { get; }
        int MovesEvaluated { get; }
        int LongestChainEvaluated { get; }
        int ShortestSolutionFound { get; }
        int LongestSolutionFound { get; }
    }
}