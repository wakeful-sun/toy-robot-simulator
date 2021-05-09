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
            ValidateState();
            Position newPosition = _locationFactory.Create(_currentPosition, StepDirection.Forward);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }

        public MoveResult Turn(RotationDirection direction)
        {
            ValidateState();
            Position newPosition = _locationFactory.Create(_currentPosition, direction);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }

        public MoveResult Report()
        {
            ValidateState();
            return MoveResult.Report(_currentPosition);
        }

        private void ValidateState()
        {
            if (_currentPosition == null)
            {
                throw new Exception("Robot is not initialized");
            }
        }
    }

    record MoveResult
    {
        public bool Successful { get; set; }
        public string Message { get; set; }

        public static MoveResult Success => new() { Successful = true };
        public static MoveResult Report(Position position) => new() { Successful = true, Message = $"{position.Coordinates.X},{position.Coordinates.Y},{position.Facing}".ToUpperInvariant() };
    }
}