using System;
using CommandHandler.Core;
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

            IApplicationSettings applicationSettings = new ApplicationSettings { MapDimensions = mapDimensions };
            ICommandHandler<TextCommand, TextCommandResponse> textCommandHandler = new ApplicationConfigurator().CreatePipeline(applicationSettings);

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