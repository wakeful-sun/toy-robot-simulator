namespace ToyRobot.Domain
{
    public interface IApplicationSettings
    {
        MapDimensions MapDimensions { get; }
    }

    public record MapDimensions(int X, int Y);
}