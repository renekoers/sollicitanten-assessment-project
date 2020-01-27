using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class TileState
    {
        public int ID { get; internal set; }
        public StateOfTile State { get; internal set; }
        public MovableItem Movable { get; internal set; }
        public TileState(int ID, StateOfTile state)
        {
            this.ID = ID;
            State = state;
        }
    }
}
