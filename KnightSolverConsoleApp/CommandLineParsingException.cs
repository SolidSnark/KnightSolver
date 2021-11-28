using CommandLine;

using System;

namespace KnightSolverConsoleApp
{
    public class CommandLineParsingException : Exception
    {
        public ParserResult<object> ParserResult { get; }

        public CommandLineParsingException(ParserResult<object> parserResult)
        {
            ParserResult = parserResult;
        }
    }
}
