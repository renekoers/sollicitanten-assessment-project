using BackEnd.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BackEnd
{
    public class Puzzle
    {
        Tile[][] AllTiles;
        ICharacter character;

        public Puzzle(string info)
        {
            // {"LevelNumber":1,"GridSize":[3,3],"Par":8,"Walls":[[1,0],[1,2]],"Begin":[2,1],"End":[0,1],"Buttons":[[1,2,0]],"Doors":[[1,1,1]],"Boxes":[[2,2]]}
            ConstructPuzzle(info);
        }

        void ConstructPuzzle(string info) 
        {
            Level l = JsonConvert.DeserializeObject<Level>(info);
            int height = l.GridSize[0];
            int width = l.GridSize[1];
            AllTiles = new Tile[height][];
            for (int i = 0; i < height; i++)
            {
                AllTiles[i] = new Tile[width];
            }
            BuildWalls(l);
            SetStart(l);
            BuildButtonsAndDoors(l);
            PlaceBoxes(l);
        }

        void BuildWalls(Level l)
        {
            int[][] walls = l.Walls;
            foreach (var x in walls)
            {
                Console.WriteLine(x);
            }
        }

        void SetStart(Level l)
        {

        }

        void BuildButtonsAndDoors(Level l)
        {

        }

        void PlaceBoxes(Level l)
        {

        }

        internal ICharacter GetCharacter()
        {
            return this.character;
        }
        internal string[][] GetState()
        {
            return new string[0][];
        }
        internal bool IsFinished()
        {
            return true;
        }
    }
}
