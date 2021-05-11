using AutoFixture;
using NSubstitute;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class LocationFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();

        private ICoordinatesFactory _coordinatesFactory;
        private IFacingProvider _facingProvider;
        private IEnumParser _enumParser;
        private LocationFactory _sut;

        private string _randomPositionText;

        [SetUp]
        public void BeforeEachTest()
        {
            _coordinatesFactory = Substitute.For<ICoordinatesFactory>();
            _facingProvider = Substitute.For<IFacingProvider>();
            _enumParser = Substitute.For<IEnumParser>();
            _sut = new LocationFactory(_coordinatesFactory, _facingProvider, _enumParser);

            _randomPositionText = CreateRandomPositionText();
        }

        [Test]
        public void ShouldParse_FacingOfPositionText_WithEnumParser()
        {
            string expectedFacingText = _fixture.Create<string>();
            string positionText = CreateRandomPositionText(facing: expectedFacingText);

            _sut.Create(positionText);

            _enumParser.Received(1).Parse<Facing>(expectedFacingText);
        }

        [Test]
        public void ShouldCreate_Coordinates_WithCoordinatesFactory_ForX()
        {
            int expectedX = _fixture.Create<int>();
            string positionText = CreateRandomPositionText(x: expectedX);

            _sut.Create(positionText);

            _coordinatesFactory.Create(expectedX, Arg.Any<int>());
        }

        [Test]
        public void ShouldCreate_Coordinates_WithCoordinatesFactory_ForY()
        {
            int expectedY = _fixture.Create<int>();
            string positionText = CreateRandomPositionText(y: expectedY);

            _sut.Create(positionText);

            _coordinatesFactory.Create(Arg.Any<int>(), expectedY);
        }

        [Test]
        public void ShouldCreate_Position_UsingEnumParserResult()
        {
            Facing expectedFacing = _fixture.Create<Facing>();
            _enumParser.Parse<Facing>(Arg.Any<string>()).Returns(expectedFacing);

            Position position = _sut.Create(_randomPositionText);

            Assert.That(position.Facing, Is.EqualTo(expectedFacing));
        }

        [Test]
        public void ShouldCreate_Position_UsingCoordinatesFactoryResult()
        {
            Coordinates expectedCoordinates = _fixture.Create<Coordinates>();
            _coordinatesFactory.Create(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedCoordinates);

            Position position = _sut.Create(_randomPositionText);

            Assert.That(position.Coordinates, Is.EqualTo(expectedCoordinates));
        }

        [Test]
        public void ShouldCall_CoordinatesFactory_WithStepDirection()
        {
            Position currentPosition = _fixture.Create<Position>();
            StepDirection stepDirection = _fixture.Create<StepDirection>();

            _sut.Create(currentPosition, stepDirection);

            _coordinatesFactory.Received(1).Create(currentPosition, stepDirection);
        }

        [Test]
        public void ShouldCall_UseCoordinates_FromCoordinatesFactory_WithStepDirection()
        {
            Position initialPosition = _fixture.Create<Position>();
            StepDirection stepDirection = _fixture.Create<StepDirection>();
            Coordinates expectedCoordinates = _fixture.Create<Coordinates>();
            _coordinatesFactory.Create(Arg.Any<Position>(), Arg.Any<StepDirection>()).Returns(expectedCoordinates);

            Position newPosition = _sut.Create(initialPosition, stepDirection);

            Assert.That(newPosition.Coordinates, Is.EqualTo(expectedCoordinates));
            Assert.That(newPosition.Facing, Is.EqualTo(initialPosition.Facing));
        }

        [Test]
        public void ShouldCall_CoordinatesFactory_WithRotationDirection()
        {
            Position currentPosition = _fixture.Create<Position>();
            RotationDirection rotationDirection = _fixture.Create<RotationDirection>();

            _sut.Create(currentPosition, rotationDirection);

            _facingProvider.Received(1).GetNext(currentPosition.Facing, rotationDirection);
        }

        [Test]
        public void ShouldCall_UseCoordinates_FromCoordinatesFactory_WithRotationDirection()
        {
            Position initialPosition = _fixture.Create<Position>();
            RotationDirection rotationDirection = _fixture.Create<RotationDirection>();
            Facing expectedFacing = _fixture.Create<Facing>();
            _facingProvider.GetNext(Arg.Any<Facing>(), Arg.Any<RotationDirection>()).Returns(expectedFacing);

            Position newPosition = _sut.Create(initialPosition, rotationDirection);

            Assert.That(newPosition.Coordinates, Is.EqualTo(initialPosition.Coordinates));
            Assert.That(newPosition.Facing, Is.EqualTo(expectedFacing));
        }



        string CreateRandomPositionText(int? x = null, int? y = null, string facing = null)
        {
            int xCoordinate = x ?? _fixture.Create<int>();
            int yCoordinate = y ?? _fixture.Create<int>();
            string facingText = facing ?? _fixture.Create<string>();

            return $"{xCoordinate},{yCoordinate},{facingText}";
        }
    }
}