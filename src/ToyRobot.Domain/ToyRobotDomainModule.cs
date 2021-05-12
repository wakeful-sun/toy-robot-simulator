using CommandHandler.Core;
using ToyRobot.Domain.ControlCenter;

namespace ToyRobot.Domain
{
    public class ToyRobotDomainModule
    {
        public ICommandHandler<TextCommand, TextCommandResponse> CreateTextCommandHandler(IApplicationSettings applicationSettings)
        {
            ICoordinatesValidator coordinatesValidator = new CoordinatesValidator();
            ILocationFactory locationFactory = new LocationFactory(new CoordinatesFactory(), new FacingProvider(), new EnumParser());
            IRobot robot = new Domain.ToyRobot(new Map(applicationSettings, coordinatesValidator), locationFactory);
            TextCommandHandler textCommandHandler = new(new RobotActionFactory(locationFactory), robot);

            return textCommandHandler;
        }

        public ICommandValidator<TextCommand> CreateTextCommandHandlerValidator()
        {
            return new TextCommandValidator();
        }
    }
}