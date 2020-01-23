using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Character : ICharacter
    {
        public Direction Direction { get; private set; }
        public Tile Position { get; private set; }
        public Movable HeldItem { get; set; }

        public Character(Tile position, Direction direction)
        {
            Position = position;
            Direction = direction;
            HeldItem = null;
        }

        public void ExecuteCommand(Command command)
        {
            switch (command)
            {
                case Command.MoveForward:
                    MoveForward();
                    break;
                case Command.RotateLeft:
                    RotateLeft();
                    break;
                case Command.RotateRight:
                    RotateRight();
                    break;
                case Command.PickUp:
                    PickUp();
                    break;
                case Command.Drop:
                    Drop();
                    break;
                default: return;
            }
        }

        protected virtual void RotateLeft()
        {
            Direction = Direction.Left();
        }

        protected virtual void RotateRight()
        {
            Direction = Direction.Right();
        }

        protected virtual bool MoveForward()
        {
            Tile tileInFront = Position.GetTile(Direction);
            if (tileInFront.Passable)
            {
                Position = tileInFront;
                return true;
            }
            return false;
        }

        protected virtual bool PickUp()
        {
            Tile tileInFront = Position.GetTile(Direction);
            if (HeldItem == null && tileInFront.ContainsMoveable)
            {
                HeldItem = tileInFront.Retrieve();
                return HeldItem != null;
            }
            return false;
        }

        protected virtual bool Drop()
        {
            Tile tileInFront = Position.GetTile(Direction);
            if (HeldItem != null && tileInFront.Passable)
            {
                if (tileInFront.DropOnto(HeldItem))
                {
                    HeldItem = null;
                    return true;
                }
            }
            return false;
        }

        public bool CheckCondition(ConditionParameter parameter, ConditionValue value)
        {
            Tile tileToCheck;
            switch (parameter)
            {
                case ConditionParameter.TileNorth:
                    tileToCheck = Position.GetNeighbor(Direction.North);
                    break;
                case ConditionParameter.TileEast:
                    tileToCheck = Position.GetNeighbor(Direction.East);
                    break;
                case ConditionParameter.TileSouth:
                    tileToCheck = Position.GetNeighbor(Direction.South);
                    break;
                case ConditionParameter.TileWest:
                    tileToCheck = Position.GetNeighbor(Direction.West);
                    break;
                case ConditionParameter.TileCurrent:
                    tileToCheck = Position;
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (value)
            {
                case ConditionValue.Passable:
                    return tileToCheck.Passable;
                case ConditionValue.Impassable:
                    return !tileToCheck.Passable;
                case ConditionValue.Button:
                    return tileToCheck is ButtonTile;
                case ConditionValue.HasMovable:
                    return tileToCheck.ContainsMoveable;
                case ConditionValue.Finish:
                    return tileToCheck is FinishTile;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
