using System;

namespace ToyRobotSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    record Position
    {
        public Coordinates Coordinates { get; set; }
        public Facing Facing { get; set; }
    }

    record Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    interface ICoordinatesFactory
    {
        Coordinates Create(int x, int y);
        Coordinates Create(Coordinates initialCoordinates, StepDirection direction);
    }
}