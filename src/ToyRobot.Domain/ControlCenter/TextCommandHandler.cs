using System;
using CommandHandler.Core;

namespace ToyRobot.Domain.ControlCenter
{
    class TextCommandHandler : ICommandHandler<TextCommand, TextCommandResponse>
    {
        private readonly IRobotActionFactory _actionFactory;
        private readonly IRobot _robot;

        public TextCommandHandler(IRobotActionFactory actionFactory, IRobot robot)
        {
            _actionFactory = actionFactory;
            _robot = robot;
        }

        public TextCommandResponse Execute(TextCommand command)
        {
            RobotAction action = _actionFactory.Create(command.Input);
            MoveResult moveResult = ExecuteRobotAction(action);

            return new TextCommandResponse(moveResult.Message);
        }

        private MoveResult ExecuteRobotAction(RobotAction action) =>
            action.ActionType switch
            {
                RobotActionType.Turn => _robot.Turn(action.RotationDirection),
                RobotActionType.Place => _robot.Place(action.NewPosition),
                RobotActionType.Move => _robot.Move(),
                RobotActionType.Report => _robot.Report(),
                _ => throw new ArgumentException($"Robot action type {action.ActionType} is not supported.")
            };
    }

    record TextCommandResponse(string Output);
    record TextCommand(string Input);
}