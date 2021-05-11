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
        private const string ExitCommandText = "exit";

        static void Main(string[] args)
        {
            MapDimensions mapDimensions = new(5, 5);
            ICommandHandler<TextCommand, TextCommandResponse> textCommandHandler = CreatePipeline(mapDimensions);

            PrintInfo(mapDimensions);

            while (true)
            {
                string input = Console.ReadLine();
                if (string.Equals(input, ExitCommandText, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                TextCommandResponse response = textCommandHandler.Execute(new TextCommand(input));
                if (!string.IsNullOrWhiteSpace(response?.Output))
                {
                    ConsoleColor initialColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\t" + response.Output);
                    Console.ForegroundColor = initialColor;
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

        private static void PrintInfo(MapDimensions mapDimensions)
        {
            string[] appInfoText =
            {
                new ('*', 90),
                "Type robot command and hit ENTER or press Ctrl+C.",
                "Supported commands are:",
                "\tPLACE X,Y,F   : puts robot in position X,Y and facing NORTH, SOUTH, EAST or WEST",
                "\tMOVE          : moves the toy robot one unit forward",
                "\tLEFT          : rotates the robot 90 degrees in LEFT direction",
                "\tRIGHT         : rotates the robot 90 degrees in RIGHT direction",
                "\tREPORT        : announces the X,Y and F of the robot.",
                $"Map size if {mapDimensions.X} units x {mapDimensions.Y} units",
                new ('*', 90)
            };

            foreach (string line in appInfoText)
            {
                Console.WriteLine(line);
            }
        }
    }
}