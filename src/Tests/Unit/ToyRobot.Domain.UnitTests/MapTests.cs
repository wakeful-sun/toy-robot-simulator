using AutoFixture;
using NSubstitute;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class MapTests
    {
        private readonly IFixture _fixture = new Fixture();

        private MapDimensions _dimensions;
        private ICoordinatesValidator _validator;
        private Map _sut;

        [SetUp]
        public void BeforeEachTest()
        {
            _dimensions = _fixture.Create<MapDimensions>();
            _validator = Substitute.For<ICoordinatesValidator>();
            _sut = new Map(_dimensions, _validator);
        }

        [Test]
        public void ShouldValidate_CoordinatesOfNewPosition_AgainstMapDimensions()
        {
            Position position = _fixture.Create<Position>();

            _sut.Move(position);

            _validator.Received(1).ValidateAndThrow(_dimensions, position.Coordinates);
        }

        [Test]
        public void ShouldReturn_PositionUpdateResult_WithNewPosition()
        {
            Position position = _fixture.Create<Position>();

            PositionUpdateResult result = _sut.Move(position);

            Assert.That(result.NewPosition, Is.SameAs(position));
        }
    }
}