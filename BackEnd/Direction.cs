using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public static class DirectionFunctionality
    {
        public static Direction Right(this Direction dir)
        {
            return (Direction)(((int)dir + 1) % 4);
        }
        public static Direction Left(this Direction dir)
        {
            return (Direction)(((int)dir + 3) % 4);
        }

        public static (int, int) OfPosition(this Direction dir, int row, int col)
        {
            switch (dir)
            {
                case Direction.North:
                    return (row - 1, col);
                case Direction.West:
                    return (row, col - 1);
                case Direction.South:
                    return (row + 1, col);
                case Direction.East:
                    return (row, col + 1);
                default:
                    return (row, col);
            }
        }
    }
}