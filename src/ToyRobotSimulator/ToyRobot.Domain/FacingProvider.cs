using System;
using System.Collections.Generic;

namespace ToyRobot.Domain
{
    interface IFacingProvider
    {
        Facing GetNext(Facing currentFacing, RotationDirection rotationDirection);
    }

    class FacingProvider : IFacingProvider
    {
        private readonly List<Facing> _orderedFacings = new() { Facing.North, Facing.East, Facing.South, Facing.West };

        public Facing GetNext(Facing currentFacing, RotationDirection rotationDirection)
        {
            if (!_orderedFacings.Contains(currentFacing))
            {
                throw new ArgumentException($"Facing {currentFacing} is not supported");
            }

            int currentFacingIndex = _orderedFacings.IndexOf(currentFacing);
            int nextFacingIndex = GetNextFacingIndex(rotationDirection, currentFacingIndex);

            Facing nextFacing = _orderedFacings[nextFacingIndex];
            return nextFacing;
        }

        private int GetNextFacingIndex(RotationDirection rotationDirection, int currentFacingIndex)
        {
            const int minIndex = 0;
            int maxIndex = _orderedFacings.Count - 1;

            switch (rotationDirection)
            {
                case RotationDirection.Left:
                    return currentFacingIndex == minIndex ? maxIndex : currentFacingIndex - 1;
                case RotationDirection.Right:
                    return currentFacingIndex == maxIndex ? minIndex : currentFacingIndex + 1;
                default:
                    throw new ArgumentException($"Rotation direction {rotationDirection} is not supported");
            }
        }
    }
}