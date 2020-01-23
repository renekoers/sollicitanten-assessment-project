using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class DoorTileState : TileState
    {
        public bool IsOpen { get; }
        public DoorTileState(StateOfTile state, bool isOpen) : base(state)
        {
            this.IsOpen = isOpen;
        }
    }
}
