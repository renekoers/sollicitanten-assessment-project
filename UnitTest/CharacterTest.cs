using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    class CharacterTest
    {
        [TestMethod]
        public void RotateLeftUpdatesDirection()
        {
            Character character = new Character(new Tile(), Direction.North);

            character.ExecuteCommand(Command.RotateLeft);

            Assert.AreEqual(Direction.West, character.Direction);
        }

        [TestMethod]
        public void RotateRightUpdatesDirection()
        {
            Character character = new Character(new Tile(), Direction.East);

            character.ExecuteCommand(Command.RotateRight);

            Assert.AreEqual(Direction.South, character.Direction);
        }
    }
}
