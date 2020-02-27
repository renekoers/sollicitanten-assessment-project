using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class LevelTest
    {
        [TestMethod]
        public void GetLevelNumberTest()
        {
            Level level = Level.Get(1);
            Assert.AreEqual(1, level.LevelNumber);
        }

        [TestMethod]
        public void LevelNumberTest()
        {
            Level level = Level.CreateFromString("##\n..\n3,2");
            Assert.AreEqual(3, level.LevelNumber);
        }

        [TestMethod]
        public void ParTest()
        {
            Level level = Level.CreateFromString("#.\n.#\n3,2");
            Assert.AreEqual(2, level.Par);
        }

        [TestMethod]
        public void WallCreationTest()
        {
            Level level = Level.CreateFromString("#\n1,1");
            CollectionAssert.AreEqual(new int[] { 0, 0 }, level.Walls[0]);
        }

        [TestMethod]
        public void FinishCreationTest()
        {
            Level level = Level.CreateFromString("#.\n!.\n1,1");
            CollectionAssert.AreEqual(new int[] { 1, 0 }, level.End);
        }

        [TestMethod]
        public void ButtonTest()
        {
            Level level = Level.CreateFromString("ab\n##\n1,1");
            Assert.AreEqual(2, level.Buttons.Length);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0 }, level.Buttons[0]);
            CollectionAssert.AreEqual(new int[] { 1, 0, 1 }, level.Buttons[1]);
        }

        [TestMethod]
        public void DoorTest()
        {
            Level level = Level.CreateFromString("AB\n##\n1,1");
            Assert.AreEqual(2, level.Doors.Length);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0 }, level.Doors[0]);
            CollectionAssert.AreEqual(new int[] { 1, 0, 1 }, level.Doors[1]);
        }

        [TestMethod]
        public void ButtonDoorTest()
        {
            Level level = Level.CreateFromString("a.\n.A\n1,1");
            Assert.AreEqual(level.Doors[0][0], level.Buttons[0][0]);
        }

        [TestMethod]
        public void BoxTest()
        {
            Level level = Level.CreateFromString("*\n1,1");
            CollectionAssert.AreEqual(new int[] { 0, 0 }, level.Boxes[0]);
        }

        [TestMethod]
        public void CharacterDirectionNorthTest()
        {
            Level level = Level.CreateFromString("^#\n##\n1,1");
            Assert.AreEqual(Direction.North, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 0, 0 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionEastTest()
        {
            Level level = Level.CreateFromString("#>\n##\n1,1");
            Assert.AreEqual(Direction.East, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 0, 1 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionSouthTest()
        {
            Level level = Level.CreateFromString("##\n_#\n1,1");
            Assert.AreEqual(Direction.South, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 1, 0 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionWestTest()
        {
            Level level = Level.CreateFromString("##\n#<\n1,1");
            Assert.AreEqual(Direction.West, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 1, 1 }, level.PositionCharacter);
        }

        [TestMethod]
        public void GridSize3By3Test()
        {
            Level level = Level.CreateFromString("###\n#.#\n###\n1,1");
            Assert.AreEqual(3, level.GridSize[0]);
            Assert.AreEqual(3, level.GridSize[1]);
        }

        [TestMethod]
        public void GridSizeHeight2Width4Test()
        {
            Level level = Level.CreateFromString("!#a#\nA.*_\n1,1");
            Assert.AreEqual(2, level.GridSize[0]);
            Assert.AreEqual(4, level.GridSize[1]);
        }
    }
}
