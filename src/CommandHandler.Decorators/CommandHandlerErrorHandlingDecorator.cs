using System;
using CommandHandler.Core;
using Microsoft.Extensions.Logging;

namespace CommandHandler.Decorators
{
    public class CommandHandlerErrorHandlingDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
        private readonly ILogger<ICommandHandler<TCommand, TResponse>> _logger;

        public CommandHandlerErrorHandlingDecorator(ICommandHandler<TCommand, TResponse> commandHandler, ILogger<ICommandHandler<TCommand, TResponse>> logger)
        {
            _commandHandler = commandHandler;
            _logger = logger;
        }

        public TResponse Execute(TCommand command)
        {
            try
            {
                TResponse response = _commandHandler.Execute(command);
                return response;
            }
            catch (Exception e)
            {
                string commandHandlerName = _commandHandler.GetType().Name;
                _logger.Log(LogLevel.Error, e, $"Processing command of {commandHandlerName} command handle has failed.");

                return default;
            }
        }
    }
}