using System;

namespace ToyRobot.Domain
{
    interface ICoordinatesFactory
    {
        Coordinates Create(int x, int y);
        Coordinates Create(Coordinates initialCoordinates, StepDirection direction);
    }

    class CoordinatesFactory : ICoordinatesFactory
    {
        public Coordinates Create(int x, int y)
        {
            throw new NotImplementedException();
        }

        public Coordinates Create(Coordinates initialCoordinates, StepDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}