namespace ToyRobotSimulator
{
    interface IRobot
    {
        Position CurrentPosition { get; }
        MoveResult Place(Position newPosition);
        MoveResult Move();
        MoveResult Turn(RotationDirection direction);
    }

    class ToyRobot : IRobot
    {
        private readonly IMap _map;
        private readonly ILocationFactory _locationFactory;

        public Position CurrentPosition { get; private set; }

        public ToyRobot(Position initialPosition, IMap map, ILocationFactory locationFactory)
        {
            CurrentPosition = initialPosition;
            _map = map;
            _locationFactory = locationFactory;
        }

        public MoveResult Place(Position newPosition)
        {
            PositionUpdateResult positionUpdate = _map.UpdateLocation(newPosition);
            CurrentPosition = positionUpdate.NewPosition;

            if (positionUpdate.Success)
            {
                return MoveResult.Success;
            }

            return MoveResult.Failure(positionUpdate.ErrorMessage);
        }

        public MoveResult Move()
        {
            Position newPosition = _locationFactory.Create(CurrentPosition, StepDirection.Forward);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }

        public MoveResult Turn(RotationDirection direction)
        {
            Position newPosition = _locationFactory.Create(CurrentPosition, direction);
            MoveResult moveResult = Place(newPosition);
            return moveResult;
        }
    }

    record MoveResult
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }

        public static MoveResult Success => new() { Successful = true };
        public static MoveResult Failure(string errorMessage) => new() { Successful = false, ErrorMessage = errorMessage };
    }
}