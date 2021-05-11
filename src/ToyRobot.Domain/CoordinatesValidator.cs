using System;

namespace ToyRobot.Domain
{
    interface ICoordinatesValidator
    {
        public void ValidateAndThrow(MapDimensions mapDimensions, Coordinates coordinates);
    }

    class CoordinatesValidator : ICoordinatesValidator
    {
        public void ValidateAndThrow(MapDimensions mapDimensions, Coordinates coordinates)
        {
            throw new NotImplementedException();
        }
    }
}