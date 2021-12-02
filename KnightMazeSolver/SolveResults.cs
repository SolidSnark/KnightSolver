using System.Collections.Generic;

namespace KnightMazeSolver
{
    public class SolveResults : ISolveResults
    {
        public SquareColor[,] Board { get; internal set; }
        public List<List<IMove>> Solutions { get; internal set; }
        public int[] NeighborCount { get; internal set; } = new int[9];
        public int MovesEvaluated { get; internal set; }
        public int LongestChainEvaluated { get; internal set; }
    }
}
