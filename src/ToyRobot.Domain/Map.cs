namespace ToyRobot.Domain
{
    interface IMap
    {
        PositionUpdateResult Move(Position newPosition);
    }

    class Map : IMap
    {
        private readonly MapDimensions _dimensions;
        private readonly ICoordinatesValidator _validator;

        public Map(MapDimensions dimensions, ICoordinatesValidator validator)
        {
            _dimensions = dimensions;
            _validator = validator;
        }

        public PositionUpdateResult Move(Position newPosition)
        {
            _validator.ValidateAndThrow(_dimensions, newPosition.Coordinates);

            return new PositionUpdateResult(newPosition);
        }
    }

    record MapDimensions(int X, int Y);
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
    };
}