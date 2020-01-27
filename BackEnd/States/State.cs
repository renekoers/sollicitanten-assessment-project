using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public enum StateOfTile { Door, Wall, Empty, Button, End}
    public enum MovableItem { Box}
    public class State : IState
    {
        public CharacterState Character { get; }
        public List<TileState> PuzzleTiles { get; }
        public int PuzzleWidth { get; }
        public int PuzzleHeight { get; }
        public int PuzzleLevel { get; }

        public State(Puzzle puzzle)
        {
            ICharacter character = puzzle.Character;
            Tile[,] tiles = puzzle.AllTiles;
            PuzzleTiles = new List<TileState>();
            PuzzleLevel = puzzle.LevelNumber;

            Dictionary<Tile, ButtonTileState> linkedDoorsWithButtons = new Dictionary<Tile, ButtonTileState>();

            PuzzleWidth = tiles.GetLength(1);
            PuzzleHeight = tiles.GetLength(0);

            Tile CharacterTile = character.Position;
            Tile EndTile = puzzle.Finish;
            Character = new CharacterState(character.Direction);
            Tile tileCharacter = character.Position;

            foreach (Tile tile in tiles)
            {
                TileState tileState;
                switch (tile.GetType().Name)
                {
                    case nameof(DoorTile):
                        tileState = new DoorTileState(StateOfTile.Door, tile.Passable);
                        break;
                    case nameof(ButtonTile):
                        tileState = new ButtonTileState(StateOfTile.Button);
                        linkedDoorsWithButtons.Add(((ButtonTile)tile).door, (ButtonTileState) tileState);
                        break;
                    case nameof(WallTile):
                        tileState = new TileState(StateOfTile.Wall);
                        break;
                    default:
                        tileState = new TileState(StateOfTile.Empty);
                        break;
                }
                if (tile.Equals(tileCharacter))
                {
                    Character.Tile = tileState;
                }
                if (tile.Equals(EndTile))
                {
                    tileState.State = StateOfTile.End;
                }
                if (tile.ContainsMoveable)
                {
                    tileState.Movable = MovableItem.Box;
                }
                PuzzleTiles.Add(tileState);
            }

            for (int rowIndex = 0; rowIndex < PuzzleHeight; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < PuzzleWidth; columnIndex++)
                {
                    Tile tile = tiles[rowIndex, columnIndex];
                    if (linkedDoorsWithButtons.TryGetValue(tile, out ButtonTileState button))
                    {
                        button.Door = (DoorTileState)PuzzleTiles[rowIndex * PuzzleWidth + columnIndex];
                    }
                }
            }
        }
    }
}
