﻿using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class DirectionTest
    {
        [TestMethod]
        public void LeftOfNorthIsWestTest()
        {
            Direction dir = Direction.North;
            Direction dirLeft = dir.Left();
            Assert.AreEqual(Direction.West, dirLeft);
        }
        [TestMethod]
        public void RightOfNorthIsEastTest()
        {
            Direction dir = Direction.North;
            Direction dirRight = dir.Right();
            Assert.AreEqual(Direction.East, dirRight);
        }

        [TestMethod]
        public void LeftOfEastIsNorthTest()
        {
            Direction dir = Direction.East;
            Direction dirLeft = dir.Left();
            Assert.AreEqual(Direction.North, dirLeft);
        }

        [TestMethod]
        public void RightOfWestIsNorthTest()
        {
            Direction dir = Direction.West;
            Direction dirRight = dir.Right();
            Assert.AreEqual(Direction.North, dirRight);
        }
        [TestMethod]
        public void FourTimesLeftOfNorthIsNorthTest()
        {
            Direction dir = Direction.North;
            for(int timesLeft=0; timesLeft<4; timesLeft++)
            {
                dir = dir.Left();
            }
            Assert.AreEqual(Direction.North, dir);
        }
        [TestMethod]
        public void FourTimesRightOfNorthIsNorthTest()
        {
            Direction dir = Direction.North;
            for(int timesRight=0; timesRight<4; timesRight++)
            {
                dir = dir.Right();
            }
            Assert.AreEqual(Direction.North, dir);
        }
    }
}
