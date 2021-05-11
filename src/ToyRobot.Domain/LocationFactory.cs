namespace ToyRobot.Domain
{
    interface ILocationFactory
    {
        Position Create(string positionText, char positionPartsDelimiter = ',');
        Position Create(Position currentPosition, StepDirection direction);
        Position Create(Position currentPosition, RotationDirection direction);
    }

    class LocationFactory : ILocationFactory
    {
        private readonly ICoordinatesFactory _coordinatesFactory;
        private readonly IFacingProvider _facingProvider;
        private readonly IEnumParser _enumParser;

        public LocationFactory(ICoordinatesFactory coordinatesFactory, IFacingProvider facingProvider, IEnumParser enumParser)
        {
            _coordinatesFactory = coordinatesFactory;
            _facingProvider = facingProvider;
            _enumParser = enumParser;
        }

        public Position Create(string positionText, char positionPartsDelimiter = ',')
        {
            string[] positionParts = positionText.Split(positionPartsDelimiter);
            int x = int.Parse(positionParts[0]);
            int y = int.Parse(positionParts[1]);
            Facing facing = _enumParser.Parse<Facing>(positionParts[2]);

            return new()
            {
                Coordinates = _coordinatesFactory.Create(x, y),
                Facing = facing
            };
        }

        public Position Create(Position currentPosition, StepDirection direction)
        {
            return new()
            {
                Coordinates = _coordinatesFactory.Create(currentPosition.Coordinates, direction),
                Facing = currentPosition.Facing
            };
        }

        public Position Create(Position currentPosition, RotationDirection direction)
        {
            Facing newFacing = _facingProvider.GetNext(currentPosition.Facing, direction);

            return new()
            {
                Coordinates = currentPosition.Coordinates,
                Facing = newFacing
            };
        }
    }

    enum StepDirection
    {
        Undefined = 0,
        Forward = 1
    }

    enum RotationDirection
    {
        Undefined = 0,
        Right = 1,
        Left = 2
    }

    enum Facing
    {
        North = 1,
        South = 2,
        East = 3,
        West = 4
    }
}