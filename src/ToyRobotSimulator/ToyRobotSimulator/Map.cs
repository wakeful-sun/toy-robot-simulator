namespace ToyRobotSimulator
{
    interface IMap
    {
        PositionUpdateResult UpdateLocation(Position newPosition);
    }

    class Map : IMap
    {
        private readonly byte[,] _space;
        private readonly ICoordinatesValidator _validator;

        public Map(byte[,] space, ICoordinatesValidator validator)
        {
            _space = space;
            _validator = validator;
        }

        public PositionUpdateResult UpdateLocation(Position newPosition)
        {
            ValidationResult validationResult = _validator.Validate(_space, newPosition.Coordinates);
            if (validationResult.IsValid)
            {
                return new PositionUpdateResult
                {
                    NewPosition = newPosition,
                    ErrorMessage = null,
                    Success = true
                };
            }

            return new PositionUpdateResult
            {
                NewPosition = newPosition,
                ErrorMessage = null,
                Success = true
            };
        }
    }

    record PositionUpdateResult
    {
        public Position NewPosition { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}