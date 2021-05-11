using System;

namespace ToyRobot.Domain
{
    interface ICoordinatesFactory
    {
        Coordinates Create(int x, int y);
        Coordinates Create(Position currentPosition, StepDirection stepDirection);
    }

    class CoordinatesFactory : ICoordinatesFactory
    {
        public Coordinates Create(int x, int y)
        {
            return new(x, y);
        }

        public Coordinates Create(Position currentPosition, StepDirection stepDirection)
        {
            Coordinates delta = CreateDelta(currentPosition, stepDirection);
            Coordinates newCoordinates = currentPosition.Coordinates + delta;

            return newCoordinates;
        }

        Coordinates CreateDelta(Position currentPosition, StepDirection stepDirection) =>
            stepDirection switch
            {
                StepDirection.Forward => CreateForwardDelta(currentPosition.Facing),
                _ => throw new ArgumentException($"Step direction {stepDirection} is not supported.")
            };

        Coordinates CreateForwardDelta(Facing facing, int stepSize = 1) =>
            facing switch
            {
                Facing.East => Create(stepSize, 0),
                Facing.West => Create(-stepSize, 0),
                Facing.North => Create(0, stepSize),
                Facing.South => Create(0, -stepSize),
                _ => throw new ArgumentException($"Facing {facing} is not supported.")
            };
    }
}