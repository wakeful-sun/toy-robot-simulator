namespace ToyRobot.Domain
{
    interface ILocationFactory
    {
        Position Create(string positionText, char positionPartsDelimiter = ',');
        Position Create(Position currentPosition, StepDirection stepDirection);
        Position Create(Position currentPosition, RotationDirection rotationDirection);
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
            Coordinates coordinates = _coordinatesFactory.Create(x, y);

            return new(coordinates, facing);
        }

        public Position Create(Position currentPosition, StepDirection stepDirection)
        {
            Coordinates coordinates = _coordinatesFactory.Create(currentPosition, stepDirection);
            return new(coordinates, currentPosition.Facing);
        }

        public Position Create(Position currentPosition, RotationDirection rotationDirection)
        {
            Facing newFacing = _facingProvider.GetNext(currentPosition.Facing, rotationDirection);
            return new(currentPosition.Coordinates, newFacing);
        }
    }

    enum StepDirection
    {
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