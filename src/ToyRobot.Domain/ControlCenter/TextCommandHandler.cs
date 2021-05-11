﻿using System;
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
            RobotAction action = _actionFactory.Create(command);
            MoveResult moveResult = ExecuteRobotAction(action);

            return new TextCommandResponse
            {
                Output = moveResult.Message
            };
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

    class TextCommandResponse
    {
        public string Output { get; set; }
    }

    class TextCommand
    {
        public string Input { get; set; }
    }
}