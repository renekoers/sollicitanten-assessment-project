using System;
using System.Collections.Generic;
using System.Text;
using BackEnd.Statements;

namespace BackEnd
{
    public interface ICharacter
    {
        void ExecuteCommand(Command command);
        bool CheckCondition(ConditionParameter parameter, ConditionValue value);
        Direction Direction { get; }
        Tile Position { get; }
        Movable HeldItem { get; set; }
    }
}
