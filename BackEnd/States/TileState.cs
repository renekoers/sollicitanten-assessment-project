using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BackEnd
{
    public class TileState
    {
        [JsonProperty("id")]
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
        public bool Equals(TileState otherTile)
        {
            if(otherTile is null)
            {
                return false;
            }
            return State.Equals(otherTile.State) && Movable.Equals(otherTile.Movable);
        }
        public override bool Equals(object obj) => Equals(obj as TileState);
        public override int GetHashCode() => ((object)this).GetHashCode();
    }
}
