using System;
using System.Diagnostics;
using CommandHandler.Core;
using Microsoft.Extensions.Logging;

namespace CommandHandler.Decorators
{
    public class CommandHandlerLoggingDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
        private readonly ILogger<ICommandHandler<TCommand, TResponse>> _logger;
        private readonly string _commandHandlerName;

        public CommandHandlerLoggingDecorator(ICommandHandler<TCommand, TResponse> commandHandler, ILogger<ICommandHandler<TCommand, TResponse>> logger)
        {
            _commandHandler = commandHandler;
            _logger = logger;
            _commandHandlerName = commandHandler.GetType().Name;
        }

        public TResponse Execute(TCommand command)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.Log(LogLevel.Information, $"START: command handler {_commandHandlerName}", command);

                TResponse response = _commandHandler.Execute(command);

                stopwatch.Stop();
                _logger.Log(LogLevel.Information, $"END: command handler {_commandHandlerName}, elapsed {stopwatch.ElapsedMilliseconds:#,#}ms", response);

                return response;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                _logger.Log(LogLevel.Error, e, $"END: command handler {_commandHandler}, elapsed {stopwatch.ElapsedMilliseconds:#,#}ms, exception {e.GetType().Name}: {e.Message}");
                throw;
            }
        }
    }
}