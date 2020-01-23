using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class TileState
    {
        public StateOfTile State { get; internal set; }
        public MovableItem Movable { get; internal set; }
        public TileState(StateOfTile state)
        {
            this.State = state;
        }
    }
}
