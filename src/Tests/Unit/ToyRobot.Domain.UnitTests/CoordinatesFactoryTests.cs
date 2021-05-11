using System;
using AutoFixture;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class CoordinatesFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();

        private CoordinatesFactory _sut;
        private Coordinates _arbitraryCoordinates;

        [SetUp]
        public void BeforeEachTest()
        {
            _sut = new CoordinatesFactory();
            _arbitraryCoordinates = _fixture.Create<Coordinates>();
        }

        [Test]
        public void ShouldThrow_ForUnsupportedFacing()
        {
            Facing unsupportedFacing = (Facing)99;
            Position position = new Position(_arbitraryCoordinates, unsupportedFacing);

            ArgumentException exception = Assert.Throws<ArgumentException>(() => _sut.Create(position, StepDirection.Forward));
            Assert.That(exception.Message, Is.EqualTo($"Facing {unsupportedFacing} is not supported."));
        }

        [Test]
        public void ShouldThrow_ForUnsupportedStepDirection()
        {
            StepDirection unsupportedStepDirection = (StepDirection)99;
            Position position = _fixture.Create<Position>();

            ArgumentException exception = Assert.Throws<ArgumentException>(() => _sut.Create(position, unsupportedStepDirection));
            Assert.That(exception.Message, Is.EqualTo($"Step direction {unsupportedStepDirection} is not supported."));
        }

        [TestCase(StepDirection.Forward, Facing.East, ExpectedResult = 1)]
        [TestCase(StepDirection.Forward, Facing.West, ExpectedResult = -1)]
        [TestCase(StepDirection.Forward, Facing.North, ExpectedResult = 0)]
        [TestCase(StepDirection.Forward, Facing.South, ExpectedResult = 0)]
        public int ShouldReturn_CoordinatesWithUpdatedXValue(StepDirection stepDirection, Facing facing)
        {
            const int initialX = 0;
            Coordinates initialCoordinates = new Coordinates(initialX, _fixture.Create<int>());
            Position position = new Position(initialCoordinates, facing);

            Coordinates newCoordinates = _sut.Create(position, stepDirection);

            return newCoordinates.X;
        }

        [TestCase(StepDirection.Forward, Facing.East, ExpectedResult = 0)]
        [TestCase(StepDirection.Forward, Facing.West, ExpectedResult = 0)]
        [TestCase(StepDirection.Forward, Facing.North, ExpectedResult = 1)]
        [TestCase(StepDirection.Forward, Facing.South, ExpectedResult = -1)]
        public int ShouldReturn_CoordinatesWithUpdatedYValue(StepDirection stepDirection, Facing facing)
        {
            const int initialY = 0;
            Coordinates initialCoordinates = new Coordinates(_fixture.Create<int>(), initialY);
            Position position = new Position(initialCoordinates, facing);

            Coordinates newCoordinates = _sut.Create(position, stepDirection);

            return newCoordinates.Y;
        }
    }
}