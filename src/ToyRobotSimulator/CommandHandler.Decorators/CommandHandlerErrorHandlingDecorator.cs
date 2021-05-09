using CommandHandler.Core;

namespace CommandHandler.Decorators
{
    public class CommandHandlerErrorHandlingDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _commandHandler;

        public CommandHandlerErrorHandlingDecorator(ICommandHandler<TCommand, TResponse> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public TResponse Execute(TCommand command)
        {
            try
            {
                TResponse response = _commandHandler.Execute(command);
                return response;
            }
            catch
            {
                return default;
            }
        }
    }
}