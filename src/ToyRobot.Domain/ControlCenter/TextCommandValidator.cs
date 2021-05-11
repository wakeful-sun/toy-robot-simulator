using System;
using CommandHandler.Core;

namespace ToyRobot.Domain.ControlCenter
{
    class TextCommandValidator : ICommandValidator<TextCommand>
    {
        public void Validate(TextCommand command)
        {
            throw new NotImplementedException();
        }
    }
}