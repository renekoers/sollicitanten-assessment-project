using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    class DirectionTest
    {
        [TestMethod]
        public void LeftOfNorthIsWest()
        {
            Direction dir = Direction.NORTH;

            Direction dirLeft = dir.Left();

            Assert.AreEqual(Direction.WEST, dirLeft);
        }

        [TestMethod]
        public void LeftOfEastIsNorth()
        {
            Direction dir = Direction.EAST;

            Direction dirLeft = dir.Left();

            Assert.AreEqual(Direction.NORTH, dirLeft);
        }

        [TestMethod]
        public void RightOfWestIsNorth()
        {
            Direction dir = Direction.WEST;

            Direction dirRight = dir.Right();

            Assert.AreEqual(Direction.NORTH, dirRight);
        }
    }
}
