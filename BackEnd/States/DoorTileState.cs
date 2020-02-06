using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class DoorTileState : TileState
    {
        public bool IsOpen { get; private set; }
        public DoorTileState(int ID, StateOfTile state, bool isOpen) : base(ID, state)
        {
            IsOpen = isOpen;
        }
        public bool Equals(DoorTileState otherDoor) => Equals((TileState) otherDoor) && IsOpen==otherDoor.IsOpen;
    }
}
