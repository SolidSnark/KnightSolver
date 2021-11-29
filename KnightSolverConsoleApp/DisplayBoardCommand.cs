using CommandLine;
using MediatR;

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KnightMazeSolver;

namespace KnightSolverConsoleApp
{
    [Verb("DisplayBoard", HelpText = "Output the supplied maze")]
    public class DisplayBoardOptions : IRequest<int>
    {
        [Option('f', "Filename", Required = true, HelpText = "The name of the file containing the maze")]
        public string Filename { get; set; }

        [Option('m', "Monochrome", Default = false, HelpText = "Force monochrome (Non-checkboard) output")]
        public bool Monochrome { get; set; }

        [Option('o', "OutputFilename", Default = "", Required = false, HelpText = "The name file to output the board to")]
        public string OutputFilename { get; set; }
    }

    public class DisplayBoardHandler : IRequestHandler<DisplayBoardOptions, int>
    {
        public async Task<int> Handle(DisplayBoardOptions request, CancellationToken cancellationToken)
        {
            Knight knight = new Knight();
            Board board = new Board(knight);

            if (!File.Exists(request.Filename))
            {
                throw new Exception($"File {request.Filename} does not exist.");
            }

            board.LoadStateData(File.ReadAllLines(request.Filename));

            ITextRenderer textRenderer = new TextRenderer();
            textRenderer.RenderBoard(board, request.Monochrome);
                        
            return await Task.FromResult(0);
        }
    }    
}
