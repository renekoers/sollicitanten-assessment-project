using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Tile
    {
        private readonly Tile[] neighbours = new Tile[4];
        public bool Passable { get; protected set; }
        protected Movable _containedItem;
        public virtual Movable ContainedItem
        {
            get => _containedItem;
            set => _containedItem = value;
        }
        public bool ContainsMoveable
        { 
            get
            {
                return ContainedItem != null;
            }
        }

        public Tile()
        {
            Passable = true;
            _containedItem = null;
        }

        public Tile GetNeighbour(Direction dir)
        {
            if (neighbours[(int)dir] != null)
            {
                return neighbours[(int)dir];
            }
            else
            {
                WallTile wall = new WallTile();
                wall.SetNeighbour(this, dir.Opposite());
                return wall;
            }
        }

        public void SetNeighbour(Tile neighbour, Direction dir)
        {
            neighbours[(int)dir] = neighbour;
        }

        public virtual Movable Retrieve()
        {
            Movable item = ContainedItem;
            ContainedItem = null;
            return item;
        }

        public virtual bool DropOnto(Movable item)
        {
            if (!ContainsMoveable)
            {
                ContainedItem = item;
                return true;
            }
            return false;
        }
    }
}
