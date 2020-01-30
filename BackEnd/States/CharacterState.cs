using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class CharacterState
    {
        public Direction DirectionCharacter { get; }
        public string DirectionCharacterString => DirectionCharacter.ToString();
        public TileState Tile { get; internal set; }
        public CharacterState(Direction direction)
        {
            this.DirectionCharacter = direction;
        }
    }
}
