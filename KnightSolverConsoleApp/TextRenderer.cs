using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KnightMazeSolver;

namespace KnightSolverConsoleApp
{
    interface ITextRenderer
    {
        void RenderBoard(Board board, bool monochrome = false);
        void RenderSolution(List<IMove> solution);
        void RenderSolutions(List<List<IMove>> solutions);
    }

    public class TextRenderer : ITextRenderer
    {
        public void RenderBoard(Board board, bool monochrome = false)
        {
            char blackSquareChar = monochrome ? 'X' : 'B';
            char whiteSquareChar = monochrome ? 'X' : 'W';

            for (byte y = 1; y <= board.Height; y++)
            {
                StringBuilder sb = new StringBuilder();

                for (byte x = 1; x <= board.Width; x++)
                {
                    if (board.StartingLocation.Equals(new BoardLocation(x, y)))
                    {
                        sb.Append('S');
                    }
                    else if (board.EndingLocation.Equals(new BoardLocation(x, y)))
                    {
                        sb.Append('E');
                    }
                    else
                    {
                        switch (board[x, y])
                        {
                            case SquareColor.Void:
                                sb.Append('.');
                                break;

                            case SquareColor.Black:
                                sb.Append(blackSquareChar);
                                break;

                            case SquareColor.White:
                                sb.Append(whiteSquareChar);
                                break;
                        }
                    }
                }

                Console.WriteLine(sb.ToString());
            }
        }

        public void RenderSolution(List<IMove> solution)
        {
            Console.WriteLine(string.Join(", ", solution.Select(m => m.ToString())));
        }

        public void RenderSolutions(List<List<IMove>> solutions)
        {
            int solutionCount = 1;
            foreach (List<IMove> solution in solutions)
            {
                Console.WriteLine($"Solution {solutionCount} of {solutions.Count}");
                RenderSolution(solution);
                Console.WriteLine(string.Empty);
            }
        }
    }
}
