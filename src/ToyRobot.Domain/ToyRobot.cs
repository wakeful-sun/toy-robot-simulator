using System;

namespace ToyRobot.Domain
{
    interface IRobot
    {
        MoveResult Place(Position newPosition);
        MoveResult Move();
        MoveResult Turn(RotationDirection direction);
        MoveResult Report();
    }

    class ToyRobot : IRobot
    {
        private readonly IMap _map;
        private readonly ILocationFactory _locationFactory;

        private Position _currentPosition;

        public ToyRobot(IMap map, ILocationFactory locationFactory)
        {
            _map = map;
            _locationFactory = locationFactory;
        }

        public MoveResult Place(Position newPosition)
        {
            PositionUpdateResult positionUpdate = _map.Move(newPosition);
            _currentPosition = positionUpdate.NewPosition;
            return MoveResult.Success;
        }

        public MoveResult Move()
        {
            ValidateCurrentState();
            Position newPosition = _locationFactory.Create(_currentPosition, StepDirection.Forward);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }

        public MoveResult Turn(RotationDirection direction)
        {
            ValidateCurrentState();
            Position newPosition = _locationFactory.Create(_currentPosition, direction);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }

        public MoveResult Report()
        {
            ValidateCurrentState();
            return MoveResult.Report(_currentPosition);
        }

        private void ValidateCurrentState()
        {
            if (_currentPosition == null)
            {
                throw new Exception("Robot is not initialized");
            }
        }
    }

    record MoveResult(bool Successful, string Message)
    {
        public static MoveResult Success => new(true, null);
        public static MoveResult Report(Position position) => new(true, $"{position.Coordinates.X},{position.Coordinates.Y},{position.Facing}".ToUpperInvariant());
    }
}