using ToyRobot.Domain;

namespace ToyRobot.SimulatorConsole
{
    class ApplicationSettings : IApplicationSettings
    {
        public MapDimensions MapDimensions { get; set; }
    }
}