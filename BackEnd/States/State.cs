﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BackEnd
{
    public enum StateOfTile { Door, Wall, Empty, Button, End }
    public enum MovableItem { None, Box }
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

            int nextID = 0;
            foreach (Tile tile in tiles)
            {
                TileState tileState;
                switch (tile.GetType().Name)
                {
                    case nameof(DoorTile):
                        tileState = new DoorTileState(nextID++, StateOfTile.Door, tile.Passable);
                        break;
                    case nameof(ButtonTile):
                        tileState = new ButtonTileState(nextID++, StateOfTile.Button);
                        linkedDoorsWithButtons.Add(((ButtonTile)tile).door, (ButtonTileState) tileState);
                        break;
                    case nameof(WallTile):
                        tileState = new TileState(nextID++, StateOfTile.Wall);
                        break;
                    default:
                        tileState = new TileState(nextID++, StateOfTile.Empty);
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
                if (tile.ContainsMovable)
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
        public bool Equals(State otherState){
            if(otherState is null)
            {
                return false;
            }
            if(PuzzleHeight != otherState.PuzzleHeight || PuzzleWidth != otherState.PuzzleWidth || PuzzleLevel != otherState.PuzzleLevel || !Character.Equals(otherState.Character)){
                return false;
            }
            return PuzzleTiles.SequenceEqual(otherState.PuzzleTiles);
        }
        public override bool Equals(object obj) => Equals(obj as State);
        public override int GetHashCode() => ((object)this).GetHashCode();
    }
}
