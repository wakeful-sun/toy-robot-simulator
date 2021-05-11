using System;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class CoordinatesValidatorTests
    {
        private CoordinatesValidator _sut;

        [SetUp]
        public void BeforeEachTest()
        {
            _sut = new CoordinatesValidator();
        }

        [TestCase(5, 5, 5, 0)]
        [TestCase(5, 5, 0, 5)]
        [TestCase(3, 3, -1, 0)]
        [TestCase(3, 3, 0, -1)]
        public void ShouldThrow_ForCoordinatesThatAreOutsideTheMap(int mapDimensionX, int mapDimensionY, int coordinateX, int coordinateY)
        {
            MapDimensions mapDimensions = new MapDimensions(mapDimensionX, mapDimensionY);
            Coordinates invalidCoordinates = new Coordinates(coordinateX, coordinateY);

            Assert.Throws<ArgumentException>(() => _sut.ValidateAndThrow(mapDimensions, invalidCoordinates));
        }

        [TestCase(5, 5, 4, 0)]
        [TestCase(5, 5, 0, 4)]
        [TestCase(3, 3, 0, 0)]
        [TestCase(3, 3, 1, 1)]
        public void ShouldNotThrow_ForCoordinatesThatAreInTheMapBoundaries(int mapDimensionX, int mapDimensionY, int coordinateX, int coordinateY)
        {
            MapDimensions mapDimensions = new MapDimensions(mapDimensionX, mapDimensionY);
            Coordinates invalidCoordinates = new Coordinates(coordinateX, coordinateY);

            Assert.DoesNotThrow(() => _sut.ValidateAndThrow(mapDimensions, invalidCoordinates));
        }
    }
}