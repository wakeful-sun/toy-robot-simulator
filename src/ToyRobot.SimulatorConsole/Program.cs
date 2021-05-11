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
            MapDimensions mapDimensions = new(5, 5);
            ICommandHandler<TextCommand, TextCommandResponse> textCommandHandler = CreatePipeline(mapDimensions);

            while (true)
            {
                string input = Console.ReadLine();
                if (string.Equals(input, "exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                TextCommandResponse response = textCommandHandler.Execute(new TextCommand(input));
                if (!string.IsNullOrWhiteSpace(response?.Output))
                {
                    Console.WriteLine(response.Output);
                }
            }
        }

        private static ICommandHandler<TextCommand, TextCommandResponse> CreatePipeline(MapDimensions mapDimensions)
        {
            ICoordinatesValidator coordinatesValidator = new CoordinatesValidator();
            ILocationFactory locationFactory = new LocationFactory(new CoordinatesFactory(), new FacingProvider(), new EnumParser());
            IRobot robot = new Domain.ToyRobot(new Map(mapDimensions, coordinatesValidator), locationFactory);
            TextCommandHandler textCommandHandler = new(new RobotActionFactory(locationFactory), robot);
            CommandHandlerValidationDecorator<TextCommand, TextCommandResponse> textCommandHandlerValidationDecorator = new(textCommandHandler, new TextCommandValidator());
            CommandHandlerLoggingDecorator<TextCommand, TextCommandResponse> textCommandHandlerLoggingDecorator = new(textCommandHandlerValidationDecorator, new Logger<ICommandHandler<TextCommand, TextCommandResponse>>(new NullLoggerFactory()));
            CommandHandlerErrorHandlingDecorator<TextCommand, TextCommandResponse> textCommandErrorHandlingDecorator = new(textCommandHandlerLoggingDecorator);

            return textCommandErrorHandlingDecorator;
        }
    }
}