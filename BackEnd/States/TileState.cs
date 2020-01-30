using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class TileState
    {
        public int ID { get; internal set; }
        public StateOfTile State { get;  internal set; }
        public string StateString => State.ToString();
        public MovableItem Movable { get; internal set; }
        public string MovableString => Movable.ToString();
        public TileState(int ID, StateOfTile state)
        {
            this.ID = ID;
            State = state;
        }
    }
}
