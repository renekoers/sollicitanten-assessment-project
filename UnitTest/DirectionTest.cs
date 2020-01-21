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
            Direction dir = Direction.North;

            Direction dirLeft = dir.Left();

            Assert.AreEqual(Direction.West, dirLeft);
        }

        [TestMethod]
        public void LeftOfEastIsNorth()
        {
            Direction dir = Direction.East;

            Direction dirLeft = dir.Left();

            Assert.AreEqual(Direction.North, dirLeft);
        }

        [TestMethod]
        public void RightOfWestIsNorth()
        {
            Direction dir = Direction.West;

            Direction dirRight = dir.Right();

            Assert.AreEqual(Direction.North, dirRight);
        }
    }
}
