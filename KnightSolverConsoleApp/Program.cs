using CommandLine;
using CommandLine.Text;
using Lamar;
using MediatR;
using MediatR.Pipeline;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnightSolverConsoleApp
{
    class Program
    {
        private static Container BuildContainer()
        {
            return new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<Program>();
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                });

                cfg.For(typeof(IPipelineBehavior<,>))
                    .Add(typeof(RequestPreProcessorBehavior<,>));
                cfg.For(typeof(IPipelineBehavior<,>))
                    .Add(typeof(RequestPostProcessorBehavior<,>));

                cfg.For<IMediator>().Use<Mediator>().Transient();
                cfg.For<ServiceFactory>().Use(ctx => ctx.GetInstance);
            });
        }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var command = GetVerb(args);

                var mediator = BuildContainer().GetInstance<IMediator>();
                return await mediator.Send(command);
            }
            catch (CommandLineParsingException e)
            {
                return await Task.FromResult(1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Encountered:\n" + e.ToString());
                return await Task.FromResult(1);
            }            
        }

        public static IRequest<int> GetVerb(string[] args)
        {
            var commands = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IRequest<int>)))
                .ToArray();

            var parserResult = Parser.Default.ParseArguments(args, commands);
            if (parserResult is Parsed<object> parser && parser.Value is IRequest<int> request)
            {
                return request;
            }

            throw new CommandLineParsingException(parserResult);
        }
    }
}
