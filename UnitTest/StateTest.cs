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
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            Assert.IsNotNull(level.Character);
        }
        [TestMethod]
        public void CharacterHasTileTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            Assert.IsNotNull(level.Character.Tile);
        }
        [TestMethod]
        public void PuzzleWidthTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            Assert.AreNotEqual(0, level.PuzzleWidth);
        }
        [TestMethod]
        public void PuzzleHeigthTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            Assert.AreNotEqual(0, level.PuzzleHeight);
        }
        [TestMethod]
        public void CorrectSizeTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            Assert.AreEqual(level.PuzzleHeight * level.PuzzleWidth, level.PuzzleTiles.Count);
        }
        [TestMethod]
        public void UniqueEndTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            int countEnd = 0;
            foreach (TileState tile in level.PuzzleTiles)
            {
                if (tile.State.Equals(StateOfTile.End))
                {
                    countEnd++;
                }
            }
            Assert.AreEqual(1, countEnd);
        }

        [TestMethod]
        public void IDUniquenessTest()
        {
            IState level = Api.StartLevelSession(Api.StartSession(), 1);
            HashSet<int> IDs = new HashSet<int>();
            foreach (TileState tile in level.PuzzleTiles)
            {
                Assert.IsTrue(IDs.Add(tile.ID));
            }
        }
        [TestMethod]
        public void StatesEqualsTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            State state1 = new State(puzzle);
            State state2 = new State(puzzle);
            Assert.AreEqual(state1,state2);
        }
    }
}
