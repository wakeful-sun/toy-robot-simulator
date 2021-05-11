using CommandHandler.Core;

namespace CommandHandler.Decorators
{
    public class CommandHandlerValidationDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _command;
        private readonly ICommandValidator<TCommand> _validator;

        public CommandHandlerValidationDecorator(ICommandHandler<TCommand, TResponse> command, ICommandValidator<TCommand> validator)
        {
            _command = command;
            _validator = validator;
        }

        public TResponse Execute(TCommand command)
        {
            _validator.Validate(command);
            return _command.Execute(command);
        }
    }
}