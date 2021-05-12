using CommandHandler.Core;
using CommandHandler.Decorators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ToyRobot.Domain;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.SimulatorConsole
{
    class ApplicationConfigurator
    {
        public ICommandHandler<TextCommand, TextCommandResponse> CreatePipeline(IApplicationSettings applicationSettings)
        {
            ToyRobotDomainModule domainModule = new ToyRobotDomainModule();

            ICommandHandler<TextCommand, TextCommandResponse> textCommandHandler = domainModule.CreateTextCommandHandler(applicationSettings);
            ICommandValidator<TextCommand> textCommandHandlerValidator = domainModule.CreateTextCommandHandlerValidator();

            ILoggerFactory loggerFactory = new NullLoggerFactory();
            ILogger<ICommandHandler<TextCommand, TextCommandResponse>> logger = new Logger<ICommandHandler<TextCommand, TextCommandResponse>>(loggerFactory);

            CommandHandlerValidationDecorator<TextCommand, TextCommandResponse> textCommandHandlerValidationDecorator = new(textCommandHandler, textCommandHandlerValidator);
            CommandHandlerLoggingDecorator<TextCommand, TextCommandResponse> textCommandHandlerLoggingDecorator = new(textCommandHandlerValidationDecorator, logger);
            CommandHandlerErrorHandlingDecorator<TextCommand, TextCommandResponse> textCommandErrorHandlingDecorator = new(textCommandHandlerLoggingDecorator, logger);

            return textCommandErrorHandlingDecorator;
        }
    }
}