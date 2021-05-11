using System;
using AutoFixture;
using NUnit.Framework;

namespace ToyRobot.Domain.UnitTests
{
    class FacingProviderTests
    {
        private readonly IFixture _fixture = new Fixture();

        private FacingProvider _sut;

        [SetUp]
        public void BeforeEachTest()
        {
            _sut = new FacingProvider();
        }

        [TestCase(RotationDirection.Left)]
        [TestCase(RotationDirection.Right)]
        public void ShouldThrow_ExceptionForNonSupportedFacing(RotationDirection rotationDirection)
        {
            Facing invalidFacing = (Facing)99;

            ArgumentException exception = Assert.Throws<ArgumentException>(() => _sut.GetNext(invalidFacing, rotationDirection));

            Assert.That(exception.Message, Does.StartWith($"Facing {invalidFacing} is not supported."));
        }

        [TestCase(RotationDirection.Undefined)]
        [TestCase((RotationDirection)99)]
        public void ShouldThrow_ExceptionForNonSupportedRotationDirection(RotationDirection invalidRotationDirection)
        {
            Facing facing = _fixture.Create<Facing>();

            ArgumentException exception = Assert.Throws<ArgumentException>(() => _sut.GetNext(facing, invalidRotationDirection));

            Assert.That(exception.Message, Does.StartWith($"Rotation direction {invalidRotationDirection} is not supported."));
        }

        [TestCase(Facing.North, ExpectedResult = Facing.East)]
        [TestCase(Facing.East, ExpectedResult = Facing.South)]
        [TestCase(Facing.South, ExpectedResult = Facing.West)]
        [TestCase(Facing.West, ExpectedResult = Facing.North)]
        public Facing ShouldRotate_FacingRightWithNoOverflow(Facing currentFacing)
        {
            Facing resultingFacing = _sut.GetNext(currentFacing, RotationDirection.Right);
            return resultingFacing;
        }

        [TestCase(Facing.North, ExpectedResult = Facing.West)]
        [TestCase(Facing.West, ExpectedResult = Facing.South)]
        [TestCase(Facing.South, ExpectedResult = Facing.East)]
        [TestCase(Facing.East, ExpectedResult = Facing.North)]
        public Facing ShouldRotate_FacingLeftWithNoOverflow(Facing currentFacing)
        {
            Facing resultingFacing = _sut.GetNext(currentFacing, RotationDirection.Left);
            return resultingFacing;
        }
    }
}