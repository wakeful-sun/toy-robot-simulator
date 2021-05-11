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

        [SetUp]
        public void BeforeEachTest()
        {
            _locationFactoryMock = Substitute.For<ILocationFactory>();
            _sut = new RobotActionFactory(_locationFactoryMock);
        }

        [TestCase("PLACE 0,0,NORTH", ExpectedResult = RobotActionType.Place)]
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
        [TestCase("PLACE 1,3,WEST", ExpectedResult = RotationDirection.Undefined)]
        [TestCase("MOVE", ExpectedResult = RotationDirection.Undefined)]
        [TestCase("REPORT", ExpectedResult = RotationDirection.Undefined)]
        public RotationDirection Should_ParseRotationDirection(string commandText)
        {
            RobotAction robotAction = _sut.Create(new TextCommand { Input = commandText });
            return robotAction.RotationDirection;
        }

        [Test]
        public void Should_CreatePositionUsingLocationFactory_ForPlaceCommandOnly()
        {
            // arrange
            int x = _fixture.Create<int>();
            int y = _fixture.Create<int>();
            Facing facing = _fixture.Create<Facing>();
            string expectedPlaceCommandArguments = $"{x},{y},{facing}";

            // act
            _sut.Create(new TextCommand { Input = $"PLACE {expectedPlaceCommandArguments}" });

            // assert
            _locationFactoryMock.Received(1).Create(expectedPlaceCommandArguments);
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
            _locationFactoryMock.DidNotReceive().Create(Arg.Any<string>());
        }
    }
}