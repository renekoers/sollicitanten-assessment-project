using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Tile
    {
        Tile[] neighbours = new Tile[4];
        Boolean passable = true;

        public Tile()
        {

        }

        public Tile GetTile(Direction dir)
        {
            // temporary return
            return new Tile();
        }

        void SetNeighbours()
        {

        }
    }
}
