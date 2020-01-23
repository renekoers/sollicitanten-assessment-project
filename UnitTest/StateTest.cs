using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void StateHasCharacterTest()
        {
            State level = Api.GetLevel(1)[0];
            Assert.AreNotEqual(null, level.Character);
        }
        [TestMethod]
        public void PuzzleWidthTest()
        {
            State level = Api.GetLevel(1)[0];
            Assert.AreNotEqual(0, level.PuzzleWidth);
        }
        [TestMethod]
        public void PuzzleHeigthTest()
        {
            State level = Api.GetLevel(1)[0];
            Assert.AreNotEqual(0, level.PuzzleHeight);
        }
        [TestMethod]
        public void CorrectSizeTest()
        {
            State level = Api.GetLevel(1)[0];
            Assert.AreEqual(level.PuzzleHeight * level.PuzzleWidth, level.PuzzleTiles.Count);
        }
        [TestMethod]
        public void UniqueEndTest()
        {
            State level = Api.GetLevel(1)[0];
            int countEnd = 0;
            foreach(TileState tile in level.PuzzleTiles)
            {
                if (tile.State.Equals(StateOfTile.End))
                {
                    countEnd++;
                }
            }
            Assert.AreEqual(1, countEnd);
        }
    }
}
