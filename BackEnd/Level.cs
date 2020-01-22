using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Level
    {
        private int[] _gridSize;
        private int _par;
        private int[][] _walls;
        private int[] _begin;
        private int[] _end;
        private int[][] _buttons;
        private int[][] _doors;
        private int[][] _boxes;
        private Level(int[] gridSize, int par, int[][] walls, int[] begin, int[] end, int[][] buttons, int[][] doors, int[][] boxes)
        {
            this._gridSize = gridSize;
            this._par = par;
            this._walls = walls;
            this._begin = begin;
            this._end = end;
            this._buttons = buttons;
            this._doors = doors;
            this._boxes = boxes;
        }
        private static Level[] levels = new Level[]{new Level(new int[] { 3, 3 }, 8, new int[][] { new int[] { 1, 0 }, new int[] { 1, 2 } }, 
                                        new int[] { 2, 1 }, new int[] { 0, 1 }, new int[][]{new int[] { 1, 2, 0 } },
                                        new int[][]{new int[] { 1, 1, 1 } }, new int[][]{new int[] { 2, 2 } })};
        public static string Get(int level)
        {
            return JsonConvert.SerializeObject(levels[level-1]);
        }
    }
}
