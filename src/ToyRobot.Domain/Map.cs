namespace ToyRobot.Domain
{
    interface IMap
    {
        PositionUpdateResult Move(Position newPosition);
    }

    class Map : IMap
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly ICoordinatesValidator _validator;

        public Map(IApplicationSettings applicationSettings, ICoordinatesValidator validator)
        {
            _applicationSettings = applicationSettings;
            _validator = validator;
        }

        public PositionUpdateResult Move(Position newPosition)
        {
            _validator.ValidateAndThrow(_applicationSettings.MapDimensions, newPosition.Coordinates);

            return new PositionUpdateResult(newPosition);
        }
    }

    record PositionUpdateResult(Position NewPosition);
    record Position(Coordinates Coordinates, Facing Facing);

    record Coordinates(int X, int Y)
    {
        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            int newX = a.X + b.X;
            int newY = a.Y + b.Y;
            return new(newX, newY);
        }
    }
}