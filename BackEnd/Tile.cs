using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Tile
    {
        Tile[] neighbours = new Tile[4];

        public Tile()
        {

        }

        public Tile getTile(Direction dir)
        {
            // temporary return
            return new Tile();
        }
    }
}
