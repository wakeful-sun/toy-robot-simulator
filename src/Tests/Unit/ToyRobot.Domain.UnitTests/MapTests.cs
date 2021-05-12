using AutoFixture;
using NSubstitute;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class MapTests
    {
        private readonly IFixture _fixture = new Fixture();

        private IApplicationSettings _applicationSettings;
        private ICoordinatesValidator _validator;
        private Map _sut;

        [SetUp]
        public void BeforeEachTest()
        {
            _applicationSettings = Substitute.For<IApplicationSettings>();
            _validator = Substitute.For<ICoordinatesValidator>();
            _sut = new Map(_applicationSettings, _validator);
        }

        [Test]
        public void ShouldValidate_CoordinatesOfNewPosition_AgainstMapDimensions()
        {
            // arrange
            Position position = _fixture.Create<Position>();
            MapDimensions mapDimensions = _fixture.Create<MapDimensions>();
            _applicationSettings.MapDimensions.Returns(mapDimensions);

            // act
            _sut.Move(position);

            // assert
            _validator.Received(1).ValidateAndThrow(mapDimensions, position.Coordinates);
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