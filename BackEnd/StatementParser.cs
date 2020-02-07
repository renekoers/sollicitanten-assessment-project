using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace BackEnd
{
    public static class StatementParser
    {
        private const string STATEMENT_TYPE_PROP = "type";
        private const string STATEMENT_ACTION_PROP = "action";

        public static IEnumerable<Statement> ParseStatementTreeJson(JsonElement statementTreeJson)
            => ParseStatementArrayJson(statementTreeJson);

        public static Statement ParseStatementJson(JsonElement statementElement)
        {
            string statementType = statementElement.GetProperty(STATEMENT_TYPE_PROP).GetString();
            string statementAction = statementElement.GetProperty(STATEMENT_ACTION_PROP).GetString();
            Statement statement = null;

            switch(statementType)
            {
                case "command":
                    statement = ParseCommandStatementJson(statementAction, statementElement);
                    break;
                case "flow":
                    statement = ParseFlowStatementJson(statementAction, statementElement);
                    break;
                default:
                    throw new JsonException($"Invalid statementType \"{statementType}\".");
            }

            return statement;
        }

        private static IEnumerable<Statement> ParseStatementArrayJson(JsonElement statementArray)
        {
            JsonElement.ArrayEnumerator arrayEnumerator = statementArray.EnumerateArray();
            foreach(JsonElement statementJsonElement in arrayEnumerator)
                yield return ParseStatementJson(statementJsonElement);
        }

        private static SingleCommand ParseCommandStatementJson(string statementAction, JsonElement statementElement)
        {
            switch(statementAction)
            {
                case "moveForward":
                    return new SingleCommand(Command.MoveForward);
                case "pickUp":
                    return new SingleCommand(Command.PickUp);
                case "drop":
                    return new SingleCommand(Command.Drop);
                case "rotate":
                    return ParseRotateCommandStatementJson(statementElement);
                default:
                    throw new JsonException($"Invalid command action \"{statementAction}\".");
            }
        }

        private static SingleCommand ParseRotateCommandStatementJson(JsonElement statementElement)
        {
            string directionString = statementElement.GetProperty("direction").GetString();

            switch(directionString)
            {
                case "left":
                    return new SingleCommand(Command.RotateLeft);
                case "right":
                    return new SingleCommand(Command.RotateRight);
                default:
                    throw new JsonException($"Invalid rotate direction \"{directionString}\".");
            }
        }

        private static Statement ParseFlowStatementJson(string statementAction, JsonElement statementElement)
        {
            switch(statementAction)
            {
                case "if":
                    return ParseIfElseStatementJson(statementElement);
                case "while":
                    return ParseWhileStatementJson(statementElement);
                default:
                    throw new JsonException($"Invalid flow action \"{statementAction}\".");
            }
        }

        private static IfElse ParseIfElseStatementJson(JsonElement statementElement)
        {
            JsonElement trueBlockElement = statementElement.GetProperty("true");
            JsonElement falseBlockElement = statementElement.GetProperty("false");
            JsonElement conditionElement = statementElement.GetProperty("condition");
            Tuple<ConditionParameter, ConditionValue> conditionStatementTuple = ParseConditionStatementJson(conditionElement);
            Statement[] trueBlock = ParseStatementArrayJson(trueBlockElement).ToArray();
            Statement[] falseBlock = ParseStatementArrayJson(falseBlockElement).ToArray();

            return new IfElse(
                conditionStatementTuple.Item1,
                conditionStatementTuple.Item2,
                true,
                trueBlock,
                falseBlock
            );
        }
        
        private static While ParseWhileStatementJson(JsonElement statementElement)
        {
            JsonElement blockElement = statementElement.GetProperty("action");
            JsonElement conditionElement = statementElement.GetProperty("condition");
            Tuple<ConditionParameter, ConditionValue> conditionStatementTuple = ParseConditionStatementJson(conditionElement);
            Statement[] block = ParseStatementArrayJson(blockElement).ToArray();

            return new While(
                conditionStatementTuple.Item1,
                conditionStatementTuple.Item2,
                true,
                block
            );
        }

        private static Tuple<ConditionParameter, ConditionValue> ParseConditionStatementJson(JsonElement conditionElement)
        {
            if(conditionElement.ValueKind == JsonValueKind.Null)
            {
                return new Tuple<ConditionParameter, ConditionValue>(
                    ConditionParameter.TileCurrent,
                    ConditionValue.Finish
                );
            }

            string targetObjectString = conditionElement.GetProperty("targetObject").GetString();
            string targetStateString = conditionElement.GetProperty("targetState").GetString();

            return new Tuple<ConditionParameter, ConditionValue>(
                ParseConditionParameter(targetObjectString),
                ParseConditionValue(targetStateString)
            );
        }

        private static ConditionParameter ParseConditionParameter(string targetObjectString)
        {
            switch(targetObjectString)
            {
                case "tile_in_front":
                    return ConditionParameter.TileFront;
                case "current_tile":
                    return ConditionParameter.TileCurrent;
                default:
                    throw new JsonException($"Invalid condition parameter \"{targetObjectString}\".");
            }
        }

        private static ConditionValue ParseConditionValue(string targetStateString)
        {
            switch(targetStateString)
            {
                case "passable":
                    return ConditionValue.Passable;
                case "is_box":
                    return ConditionValue.HasMovable;
                case "is_button":
                    return ConditionValue.Button;
                default:
                    throw new JsonException($"Invalid condition value \"{targetStateString}\".");
            }
        }
    }
}