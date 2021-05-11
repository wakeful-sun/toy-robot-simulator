using System;
using CommandHandler.Core;

namespace ToyRobot.Domain.ControlCenter
{
    class TextCommandValidator : ICommandValidator<TextCommand>
    {
        public void Validate(TextCommand command)
        {
            if (string.IsNullOrWhiteSpace(command?.Input))
            {
                throw new ArgumentException($"Command input expected to have value.");
            }
        }
    }
}