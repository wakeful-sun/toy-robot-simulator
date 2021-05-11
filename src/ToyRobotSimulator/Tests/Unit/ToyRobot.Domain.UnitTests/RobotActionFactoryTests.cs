using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.Domain.UnitTests
{
    class RobotActionFactoryTests
    {
        private readonly Fixture _fixture = new();

        private RobotActionFactory _sut;
        private ILocationFactory _locationFactoryMock;
        private IEnumParser _enumParser;

        [SetUp]
        public void BeforeEachTest()
        {
            _locationFactoryMock = Substitute.For<ILocationFactory>();
            _enumParser = Substitute.For<IEnumParser>();
            _sut = new RobotActionFactory(_locationFactoryMock, _enumParser);
        }

        [TestCase("PLACE", ExpectedResult = RobotActionType.Place)]
        [TestCase("MOVE", ExpectedResult = RobotActionType.Move)]
        [TestCase("LEFT", ExpectedResult = RobotActionType.Turn)]
        [TestCase("RIGHT", ExpectedResult = RobotActionType.Turn)]
        [TestCase("REPORT", ExpectedResult = RobotActionType.Report)]
        public RobotActionType Should_ParseActionType(string commandText)
        {
            RobotAction robotAction = _sut.Create(new TextCommand { Input = commandText });
            return robotAction.ActionType;
        }

        [TestCase("LEFT", ExpectedResult = RotationDirection.Left)]
        [TestCase("RIGHT", ExpectedResult = RotationDirection.Right)]
        [TestCase("PLACE", ExpectedResult = RotationDirection.Undefined)]
        [TestCase("MOVE", ExpectedResult = RotationDirection.Undefined)]
        [TestCase("REPORT", ExpectedResult = RotationDirection.Undefined)]
        public RotationDirection Should_ParseRotationDirection(string commandText)
        {
            RobotAction robotAction = _sut.Create(new TextCommand { Input = commandText });
            return robotAction.RotationDirection;
        }

        [Test]
        public void Should_ParseFacingUsingEnumParser_ForPlaceCommandOnly()
        {
            string expectedFacing = _fixture.Create<Facing>().ToString().ToUpperInvariant();

            _sut.Create(new TextCommand { Input = $"PLACE 0,0,{expectedFacing}" });

            _enumParser.Received(1).Parse<Facing>(expectedFacing);
        }

        [Test]
        public void Should_CreatePositionUsingLocationFactory_ForPlaceCommandOnly()
        {
            // arrange
            int expectedX = _fixture.Create<int>();
            int expectedY = _fixture.Create<int>();
            Facing expectedFacing = _fixture.Create<Facing>();
            string expectedFacingText = expectedFacing.ToString().ToUpperInvariant();
            _enumParser.Parse<Facing>(expectedFacingText).Returns(expectedFacing);

            // act
            _sut.Create(new TextCommand { Input = $"PLACE {expectedX},{expectedY},{expectedFacingText}" });

            // assert
            _locationFactoryMock.Received(1).Create(expectedX, expectedY, expectedFacing);
        }

        [TestCase("MOVE")]
        [TestCase("LEFT")]
        [TestCase("RIGHT")]
        [TestCase("REPORT")]
        public void ShouldNot_CreatePositionLocation(string commandText)
        {
            RobotAction robotAction = _sut.Create(new TextCommand { Input = commandText });
            Assert.That(robotAction.NewPosition, Is.Null);
        }

        [TestCase("MOVE")]
        [TestCase("LEFT")]
        [TestCase("RIGHT")]
        [TestCase("REPORT")]
        public void ShouldNot_CallPositionLocationFactory(string commandText)
        {
            _sut.Create(new TextCommand { Input = commandText });
            _locationFactoryMock.DidNotReceive().Create(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Facing>());
        }
    }
}