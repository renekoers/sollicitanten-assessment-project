using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Level
    {
        public int LevelNumber;
        public readonly int[] GridSize;
        public readonly int Par;
        public readonly int[][] Walls;
        public readonly int[] PositionCharacter;
        public readonly int[] End;
        public readonly int[][] Buttons;
        public readonly int[][] Doors;
        public readonly int[][] Boxes;

        [JsonConstructor]
        private Level(int level, int[] gridSize, int par, int[][] walls, int[] positionCharacter, int[] end, int[][] buttons, int[][] doors, int[][] boxes)
        {
            this.LevelNumber = level;
            this.GridSize = gridSize;
            this.Par = par;
            this.Walls = walls;
            this.PositionCharacter = positionCharacter;
            this.End = end;
            this.Buttons = buttons;
            this.Doors = doors;
            this.Boxes = boxes;
        }

        private static Level[] levels = new Level[]{new Level(1,new int[] { 3, 3 }, 8, new int[][] { new int[] { 1, 0 }, new int[] { 1, 2 } }, 
                                        new int[] { 2, 1 }, new int[] { 0, 1 }, new int[][]{new int[] { 1, 2, 0 } },
                                        new int[][]{new int[] { 1, 1, 1 } }, new int[][]{new int[] { 2, 2 } })};
        public static string Get(int level)
        {
            return JsonConvert.SerializeObject(levels[level - 1]);
        }
    }
}
