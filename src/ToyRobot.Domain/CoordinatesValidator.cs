using System;

namespace ToyRobot.Domain
{
    interface ICoordinatesValidator
    {
        /// <summary>
        /// The SOUTH WEST most corner of the map considered to have (0,0) coordinates
        /// </summary>
        /// <param name="mapDimensions">Map size</param>
        /// <param name="coordinates">Coordinates that need to be validated</param>
        public void ValidateAndThrow(MapDimensions mapDimensions, Coordinates coordinates);
    }

    class CoordinatesValidator : ICoordinatesValidator
    {
        public void ValidateAndThrow(MapDimensions mapDimensions, Coordinates coordinates)
        {
            const int originX = 0;
            const int originY = 0;

            bool xCoordinateIsOutsideTheMap = coordinates.X < originX || coordinates.X >= mapDimensions.X;
            bool yCoordinateIsOutsideTheMap = coordinates.Y < originY || coordinates.Y >= mapDimensions.Y;

            if (xCoordinateIsOutsideTheMap || yCoordinateIsOutsideTheMap)
            {
                throw new ArgumentException($"Given coordinates [{coordinates.X}, {coordinates.Y}] are outside of the map with [{mapDimensions.X}, {mapDimensions.Y}] size");
            }
        }
    }
}