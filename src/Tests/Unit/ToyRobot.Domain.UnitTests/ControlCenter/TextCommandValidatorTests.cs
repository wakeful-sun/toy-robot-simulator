using System;
using NUnit.Framework;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.Domain.UnitTests.ControlCenter
{
    class TextCommandValidatorTests
    {
        private TextCommandValidator _sut;

        [SetUp]
        public void BeforeEachTest()
        {
            _sut = new TextCommandValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("     ")]
        public void ShouldThrow_ForInvalidCommandInput(string invalidCommandInput)
        {
            TextCommand command = new TextCommand(invalidCommandInput);

            Assert.Throws<ArgumentException>(() => _sut.Validate(command));
        }

        [TestCase("PLACE 45,98,NORTH")]
        [TestCase("MOVE")]
        [TestCase("LEFT")]
        [TestCase("RIGHT")]
        [TestCase("REPORT")]
        public void ShouldNotThrow_ForValidCommandInput(string validCommandInput)
        {
            TextCommand command = new TextCommand(validCommandInput);

            Assert.DoesNotThrow(() => _sut.Validate(command));
        }
    }
}