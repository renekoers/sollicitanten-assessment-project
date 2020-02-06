using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class ButtonTileState : TileState
    {
        public DoorTileState Door { get; internal set; }
        public ButtonTileState(int ID, StateOfTile state) : base(ID, state)
        {
        }
        public bool Equals(ButtonTileState otherButton) => Equals((TileState) otherButton) && Door.Equals(otherButton.Door);
    }
}
