using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class CharacterState
    {
        public Direction DirectionCharacter { get; }
        public TileState tile { get; internal set; }
        public CharacterState(Direction direction)
        {
            this.DirectionCharacter = direction;
        }
    }
}
