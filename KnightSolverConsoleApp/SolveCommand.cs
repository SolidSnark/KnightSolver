using CommandLine;
using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KnightMazeSolver;

namespace KnightSolverConsoleApp
{
    [Verb("solve", HelpText = "Solve the given maze and output the soluton(s).\n\n" +
        "There must be at least 5 rows, at least 5 characters wide and all must be the same length.\n" +
        "The key is as follows:\n" +
        "  . = Void, invalid board square\n" +
        "  X - Valid board square\n" +
        "  S - Starting Location\n" +
        "  E - Ending Location")]
    public class SolveOptions : IRequest<int>
    {
        [Option('a', "Findall", Default = false, HelpText = "Find all solutions", SetName = "FindAll")]
        public bool FindAll { get; set; }

        [Option('s', "Shortest", Default = false, HelpText = "Find shortest solution", SetName = "Shortest")]
        public bool Shortest { get; set; }

        [Option('f', "Filename", Required = true, HelpText = "The name of the file containing the maze")]
        public string Filename { get; set; }

        [Option('b', "OutputBoard", Default = "", Required = false, HelpText = "Display the board with the solution")]
        public bool OutputBoard { get; set; }

        [Option('o', "OutputFilename", Default = "", Required = false, HelpText = "The file to output the solution to")]
        public string OutputFilename { get; set; }
    }

    public class SolveHandler : IRequestHandler<SolveOptions, int>
    {
        public async Task<int> Handle(SolveOptions request, CancellationToken cancellationToken)
        {
            Solver solver = new Solver();

            SolveType solveType = SolveType.Full;

            if (!request.FindAll)
            {
                solveType = SolveType.First;
            }
            else if (request.Shortest)
            {
                solveType = SolveType.Shortest;
            }

            List<List<IMove>> solutions = solver.Solve(request.Filename, solveType);

            ITextRenderer textRenderer = new TextRenderer();

            if (request.OutputBoard)
            {
                textRenderer.RenderBoard(board);
            }

            textRenderer.RenderSolutions(solutions);

            return await Task.FromResult(0);
        }
    }
}
