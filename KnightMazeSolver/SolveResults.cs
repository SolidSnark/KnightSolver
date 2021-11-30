using System;
using System.Collections.Generic;
using System.Text;

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

    public class SolveResults : ISolveResults
    {
        public SquareColor[,] Board { get; internal set; }
        public List<List<IMove>> Solutions { get; internal set; }
        public int[] NeighborCount { get; internal set; } = new int[9];
        public int MovesEvaluated { get; internal set; }
        public int LongestChainEvaluated { get; internal set; }
    }
}
