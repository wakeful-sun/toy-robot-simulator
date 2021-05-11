using System;
using System.Linq;

namespace ToyRobot.Domain
{
    interface IRobotActionFactory
    {
        RobotAction Create(string textCommand);
    }

    class RobotActionFactory : IRobotActionFactory
    {
        private readonly ILocationFactory _locationFactory;

        public RobotActionFactory(ILocationFactory locationFactory)
        {
            _locationFactory = locationFactory;
        }

        public RobotAction Create(string textCommand)
        {
            const char commandArgumentsSeparator = ' ';

            string inputText = textCommand.Trim();
            string[] commandParts = inputText.Split(commandArgumentsSeparator);
            var command = new
            {
                Text = commandParts[0].ToUpperInvariant(),
                Arguments = string.Join(null, commandParts.Skip(1))
            };

            RobotActionType robotActionType = ReadCommandAction(command.Text);
            RotationDirection rotationDirection = ReadRotationDirection(command.Text);
            Position position = null;

            if (robotActionType == RobotActionType.Place)
            {
                position = _locationFactory.Create(command.Arguments);
            }

            return new RobotAction(robotActionType, rotationDirection, position);
        }

        private RotationDirection ReadRotationDirection(string commandText) =>
            commandText switch
            {
                "LEFT" => RotationDirection.Left,
                "RIGHT" => RotationDirection.Right,
                _ => RotationDirection.Undefined
            };

        private RobotActionType ReadCommandAction(string commandText) =>
            commandText switch
            {
                "PLACE" => RobotActionType.Place,
                "MOVE" => RobotActionType.Move,
                "REPORT" => RobotActionType.Report,
                "LEFT" => RobotActionType.Turn,
                "RIGHT" => RobotActionType.Turn,
                _ => throw new ArgumentException($"Command {commandText} is not supported")
            };
    }

    record RobotAction(RobotActionType ActionType, RotationDirection RotationDirection, Position NewPosition)
    {
    }

    enum RobotActionType
    {
        Place = 1,
        Move = 2,
        Turn = 3,
        Report = 4
    }
}