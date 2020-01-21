using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public enum Direction
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
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
    }
}