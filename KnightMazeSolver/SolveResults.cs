using System.Collections.Generic;

namespace KnightMazeSolver
{
    public class SolveResults : ISolveResults
    {
        public IBoard Board { get; internal set; }
        public List<List<IMove>> Solutions { get; internal set; }
        public int MovesEvaluated { get; internal set; }
        public int LongestChainEvaluated { get; internal set; }
        public int ShortestSolutionFound { get; internal set; }
        public int LongestSolutionFound { get; internal set; }
    }
}
