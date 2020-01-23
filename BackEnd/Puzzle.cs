using BackEnd.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BackEnd
{
    public class Puzzle
    {
        private Tile[,] AllTiles;
        private ICharacter Character;
        private Tile Finish;

        public Puzzle(string info)
        {
            // {"LevelNumber":1,"GridSize":[3,3],"Par":8,"Walls":[[1,0],[1,2]],"Begin":[2,1],"End":[0,1],"Buttons":[[1,2,0]],"Doors":[[1,1,1]],"Boxes":[[2,2]]}
            ConstructPuzzle(JsonConvert.DeserializeObject<Level>(info));
        }

        void ConstructPuzzle(Level level)
        {
            int height = level.GridSize[0];
            int width = level.GridSize[1];
            AllTiles = new Tile[height, width];

            BuildWalls(level);
            BuildButtonsAndDoors(level);
            CreatePassableTiles(level);
            PlaceBoxes(level);
            CreateCharacter(level);
            SetFinish(level);
            SetNeighbours();
        }

        void BuildWalls(Level level)
        {
            int[][] walls = level.Walls;
            foreach (var wall in walls)
            {
                AllTiles[wall[0], wall[1]] = new WallTile();
            }
        }

        void BuildButtonsAndDoors(Level level)
        {
            int[][] buttons = level.Buttons;
            int[][] doors = level.Doors;
            foreach (var button in buttons)
            {
                foreach (var door in doors)
                {
                    if (button[0] == door[0])
                    {
                        DoorTile doorTile = new DoorTile();
                        AllTiles[door[1], door[2]] = doorTile;
                        AllTiles[button[1], button[2]] = new ButtonTile(doorTile);
                        break;
                    }
                }

            }
        }

        void PlaceBoxes(Level level)
        {
            int[][] boxes = level.Boxes;
            foreach (var box in boxes)
            {
                Tile tile = AllTiles[box[0], box[1]];
                if (Character.Position == tile)
                {
                    Character.HeldItem = new Box();
                }
                else
                {
                    tile.ContainedItem = new Box();
                }
            }
        }

        void CreatePassableTiles(Level level)
        {
            for (int r = 0; r < AllTiles.GetLength(0); r++)
            {
                for (int c = 0; c < AllTiles.GetLength(1); c++)
                {
                    if (AllTiles[r, c] == null)
                    {
                        AllTiles[r, c] = new Tile();
                    }
                }
            }
        }

        void SetFinish(Level level)
        {
            int[] finish = level.End;
            Finish = AllTiles[finish[0], finish[1]];
        }

        void CreateCharacter(Level level)
        {
            int[] start = level.PositionCharacter;
            Character = new Character(AllTiles[start[0], start[1]], level.DirectionCharacter);
        }

        void SetNeighbours()
        {
            for (int r = 0; r < AllTiles.GetLength(0); r++)
            {
                for (int c = 0; c < AllTiles.GetLength(1); c++)
                {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        (int row, int col) = dir.OfPosition(r, c);
                        if (row >= 0 && col >= 0 && row < AllTiles.GetLength(0) && col < AllTiles.GetLength(1))
                        {
                            AllTiles[r, c].SetNeighbour(AllTiles[row, col], dir);
                        }
                    }
                }
            }
        }
    }
}
