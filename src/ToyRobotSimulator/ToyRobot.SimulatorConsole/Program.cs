using System;
using CommandHandler.Core;
using CommandHandler.Decorators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ToyRobot.Domain;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.SimulatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MapDimensions mapDimensions = new() { X = 5, Y = 5 };
            ICommandHandler<TextCommand, TextCommandResponse> textCommandHandler = CreatePipeline(mapDimensions);

            while (true)
            {
                string input = Console.ReadLine();
                if (string.Equals(input, "exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                TextCommandResponse response = textCommandHandler.Execute(new TextCommand { Input = input });
                if (!string.IsNullOrWhiteSpace(response?.Output))
                {
                    Console.WriteLine(response.Output);
                }
            }
        }

        private static ICommandHandler<TextCommand, TextCommandResponse> CreatePipeline(MapDimensions mapDimensions)
        {
            ICoordinatesValidator coordinatesValidator = new CoordinatesValidator();
            IRobot robot = new Domain.ToyRobot(new Map(mapDimensions, coordinatesValidator), new LocationFactory(new CoordinatesFactory(), new FacingProvider()));
            TextCommandHandler textCommandHandler = new(new RobotActionFactory(), robot);
            CommandHandlerValidationDecorator<TextCommand, TextCommandResponse> textCommandHandlerValidationDecorator = new(textCommandHandler, new TextCommandValidator());
            CommandHandlerLoggingDecorator<TextCommand, TextCommandResponse> textCommandHandlerLoggingDecorator = new(textCommandHandlerValidationDecorator, new Logger<ICommandHandler<TextCommand, TextCommandResponse>>(new NullLoggerFactory()));
            CommandHandlerErrorHandlingDecorator<TextCommand, TextCommandResponse> textCommandErrorHandlingDecorator = new(textCommandHandlerLoggingDecorator);

            return textCommandErrorHandlingDecorator;
        }
    }
}