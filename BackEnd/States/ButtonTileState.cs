using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class ButtonTileState : TileState
    {
        public DoorTileState Door { get; internal set; }
        public ButtonTileState(StateOfTile state) : base(state)
        {

        }
    }
}
