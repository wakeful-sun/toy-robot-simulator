using System;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.Domain
{
    interface IRobotActionFactory
    {
        RobotAction Create(TextCommand textCommand);
    }

    class RobotActionFactory : IRobotActionFactory
    {
        private readonly ILocationFactory _locationFactory;
        private readonly IEnumParser _enumParser;

        public RobotActionFactory(ILocationFactory locationFactory, IEnumParser enumParser)
        {
            _locationFactory = locationFactory;
            _enumParser = enumParser;
        }

        public RobotAction Create(TextCommand textCommand)
        {
            throw new NotImplementedException();
        }
    }

    record RobotAction
    {
        public RobotActionType ActionType { get; set; }
        public RotationDirection RotationDirection { get; set; }
        public Position NewPosition { get; set; }
    }

    enum RobotActionType
    {
        Undefined = 0,
        Place = 1,
        Move = 2,
        Turn = 3,
        Report = 4
    }
}