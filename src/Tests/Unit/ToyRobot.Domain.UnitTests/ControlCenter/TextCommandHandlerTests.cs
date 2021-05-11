using System;
using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.Domain.UnitTests.ControlCenter
{
    class TextCommandHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IRobotActionFactory _robotActionFactory;
        private IRobot _robot;
        private TextCommandHandler _sut;
        private TextCommand _textCommand;

        [SetUp]
        public void BeforeEachTest()
        {
            _robotActionFactory = Substitute.For<IRobotActionFactory>();
            _robotActionFactory.Create(Arg.Any<string>()).Returns(_fixture.Create<RobotAction>());

            _robot = Substitute.For<IRobot>();
            MoveResult moveResult = _fixture.Create<MoveResult>();
            _robot.Place(Arg.Any<Position>()).Returns(moveResult);
            _robot.Move().Returns(moveResult);
            _robot.Turn(Arg.Any<RotationDirection>()).Returns(moveResult);
            _robot.Report().Returns(moveResult);

            _sut = new TextCommandHandler(_robotActionFactory, _robot);
            _textCommand = _fixture.Create<TextCommand>();
        }

        [Test]
        public void ShouldCreate_RobotActionOutOfGivenTextCommand()
        {
            TextCommand textCommand = _fixture.Create<TextCommand>();

            _sut.Execute(textCommand);

            _robotActionFactory.Received(1).Create(textCommand.Input);
        }

        [Test]
        public void ShouldInvoke_PlaceMethodOnRobot_ForPlaceRobotActionType()
        {
            // arrange
            RotationDirection direction = _fixture.Create<RotationDirection>();
            Position position = _fixture.Create<Position>();
            RobotAction action = new RobotAction(RobotActionType.Place, direction, position);
            _robotActionFactory.Create(Arg.Any<string>()).Returns(action);

            // act
            _sut.Execute(_textCommand);

            // assert
            _robot.Received(1).Place(position);
        }

        [Test]
        public void ShouldInvoke_TurnMethodOnRobot_ForTurnRobotActionType()
        {
            // arrange
            RotationDirection direction = _fixture.Create<RotationDirection>();
            Position position = _fixture.Create<Position>();
            RobotAction action = new RobotAction(RobotActionType.Turn, direction, position);
            _robotActionFactory.Create(Arg.Any<string>()).Returns(action);

            // act
            _sut.Execute(_textCommand);

            // assert
            _robot.Received(1).Turn(direction);
        }

        [Test]
        public void ShouldInvoke_MoveMethodOnRobot_ForMoveRobotActionType()
        {
            // arrange
            RotationDirection direction = _fixture.Create<RotationDirection>();
            Position position = _fixture.Create<Position>();
            RobotAction action = new RobotAction(RobotActionType.Move, direction, position);
            _robotActionFactory.Create(Arg.Any<string>()).Returns(action);

            // act
            _sut.Execute(_textCommand);

            // assert
            _robot.Received(1).Move();
        }

        [Test]
        public void ShouldInvoke_ReportMethodOnRobot_ForReportRobotActionType()
        {
            // arrange
            RotationDirection direction = _fixture.Create<RotationDirection>();
            Position position = _fixture.Create<Position>();
            RobotAction action = new RobotAction(RobotActionType.Report, direction, position);
            _robotActionFactory.Create(Arg.Any<string>()).Returns(action);

            // act
            _sut.Execute(_textCommand);

            // assert
            _robot.Received(1).Report();
        }

        [Test]
        public void ShouldThrow_ForNonSupportedRobotActionType()
        {
            // arrange
            RotationDirection direction = _fixture.Create<RotationDirection>();
            Position position = _fixture.Create<Position>();
            RobotAction action = new RobotAction((RobotActionType)99, direction, position);
            _robotActionFactory.Create(Arg.Any<string>()).Returns(action);

            // act
            Assert.Throws<ArgumentException>(() => _sut.Execute(_textCommand));
        }

        [Test]
        public void ShouldReturn_MessageFromRobotMoveResult()
        {
            // arrange
            MoveResult expectedMoveResult = _fixture.Create<MoveResult>();
            _robot.Place(Arg.Any<Position>()).Returns(expectedMoveResult);
            _robot.Move().Returns(expectedMoveResult);
            _robot.Turn(Arg.Any<RotationDirection>()).Returns(expectedMoveResult);
            _robot.Report().Returns(expectedMoveResult);

            // act
            TextCommandResponse actualResult = _sut.Execute(_textCommand);

            // assert
            Assert.That(actualResult.Output, Is.EqualTo(expectedMoveResult.Message));
        }
    }
}