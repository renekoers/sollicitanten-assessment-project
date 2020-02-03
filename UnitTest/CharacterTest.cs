using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CharacterTest
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

        [TestMethod]
        public void RetrieveFromTile()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Movable item = new Box();
            tileNeighbour.ContainedItem = item;
            Character character = new Character(tile, Direction.North);

            character.ExecuteCommand(Command.PickUp);

            Assert.AreEqual(item, character.HeldItem);
            Assert.IsNull(tileNeighbour.ContainedItem);
            Assert.IsFalse(tileNeighbour.ContainsMovable);
        }

        [TestMethod]
        public void DropOntoTile()
        {
            Tile tile = new Tile();
            Tile tileNeighbour = new Tile();
            tile.SetNeighbour(tileNeighbour, Direction.North);
            Movable item = new Box();
            Character character = new Character(tile, Direction.North);
            character.HeldItem = item;

            character.ExecuteCommand(Command.Drop);

            Assert.AreEqual(item, tileNeighbour.ContainedItem);
            Assert.IsTrue(tileNeighbour.ContainsMovable);
            Assert.IsNull(character.HeldItem);
        }

        [TestMethod]
        public void CannotDropOntoWall()
        {
            Tile tile = new Tile();
            Tile wall = new WallTile();
            tile.SetNeighbour(wall, Direction.North);
            Movable item = new Box();
            Character character = new Character(tile, Direction.North);
            character.HeldItem = item;

            character.ExecuteCommand(Command.Drop);

            Assert.IsFalse(wall.ContainsMovable);
            Assert.IsNull(wall.ContainedItem);
            Assert.AreEqual(item, character.HeldItem);
        }

        [TestMethod]
        public void DroppingBoxOntoButtonOpensDoor()
        {
            Tile tile = new Tile();
            DoorTile door = new DoorTile();
            ButtonTile button = new ButtonTile(door);
            tile.SetNeighbour(button, Direction.North);
            Movable item = new Box();
            Character character = new Character(tile, Direction.North);
            character.HeldItem = item;

            character.ExecuteCommand(Command.Drop);

            Assert.IsTrue(door.IsOpen);
            Assert.IsTrue(button.ContainsMovable);
            Assert.AreEqual(item, button.ContainedItem);
            Assert.IsNull(character.HeldItem);
        }

        [TestMethod]
        public void TakingItemFromButtonClosesDoor()
        {
            Tile tile = new Tile();
            DoorTile door = new DoorTile();
            ButtonTile button = new ButtonTile(door);
            tile.SetNeighbour(button, Direction.North);
            Movable item = new Box();
            button.ContainedItem = item;
            Character character = new Character(tile, Direction.North);

            character.ExecuteCommand(Command.PickUp);

            Assert.IsTrue(door.IsClosed);
            Assert.IsFalse(button.ContainsMovable);
            Assert.AreEqual(item, character.HeldItem);
        }
    }
}
