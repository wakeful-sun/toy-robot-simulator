using System;

namespace ToyRobotSimulator
{
    interface ICoordinatesValidator
    {
        public ValidationResult Validate(byte[,] space, Coordinates coordinates);
    }

    class CoordinatesValidator : ICoordinatesValidator
    {
        public ValidationResult Validate(byte[,] space, Coordinates coordinates)
        {
            throw new NotImplementedException();
        }
    }

    class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }

}