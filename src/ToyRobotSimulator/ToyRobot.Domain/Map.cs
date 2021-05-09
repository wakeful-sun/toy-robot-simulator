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

            return new PositionUpdateResult
            {
                NewPosition = newPosition
            };
        }
    }

    record MapDimensions
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    record PositionUpdateResult
    {
        public Position NewPosition { get; set; }
    }

    record Position
    {
        public Coordinates Coordinates { get; set; }
        public Facing Facing { get; set; }
    }

    record Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}