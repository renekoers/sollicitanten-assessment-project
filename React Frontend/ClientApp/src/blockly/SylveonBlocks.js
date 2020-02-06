import Blockly from "blockly";

class SylveonBlocks
{
    static get COMMAND_COLOR() { return 0; }
    static get FLOW_COLOR() { return 225; }

    static registerBlocks()
    {
        SylveonBlocks.registerBlock("move_forward", SylveonBlocks.moveForwardAction, (block) => "moveForward\n");
        SylveonBlocks.registerBlock("rotate", SylveonBlocks.rotateAction, (block) => {
            const direction = block.getFieldValue("direction");
            return "rotate_" + direction + "\n";
        });
        SylveonBlocks.registerBlock("pickup", SylveonBlocks.pickupAction, (block) => "pickUp\n");
        SylveonBlocks.registerBlock("drop", SylveonBlocks.dropAction, (block) => "drop\n");
        SylveonBlocks.registerBlock("if_then", SylveonBlocks.ifFlow, (block) => {
            const statement = Blockly.JavaScript.statementToCode(block, "statement");
            const true_content = Blockly.JavaScript.statementToCode(block, "true_action");
            const false_content = Blockly.JavaScript.statementToCode(block, "false_action");

            return "if\n" + statement + true_content + "else\n" + false_content + "end\n";
        });
        SylveonBlocks.registerBlock("while_do", SylveonBlocks.whileFlow, (block) => {
            const statement = Blockly.JavaScript.statementToCode(block, "statement");
            const content = Blockly.JavaScript.statementToCode(block, "action");

            return "while\n" + statement + content + "end\n";
        });
        SylveonBlocks.registerBlock("state_equals", SylveonBlocks.flowStatementStateEquals, (block) => {
            const object = block.getFieldValue("flow_statement_object");
            const objectState = block.getFieldValue("flow_statement_object_state");

            return object + "\n" + objectState + "\n";
        });
    }

    static registerBlock(name, object, writeCode)
    {
        Blockly.Blocks[name] = {
            init: function() {
                this.jsonInit(object);
            }
        }

        Blockly.JavaScript[name] = function(block) { return writeCode(block); }
    }

    static get moveForwardAction()
    {
        return {
            message0: "Move forward",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get rotateAction()
    {
        return {
            message0: "Rotate %1",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
            args0: [
                {
                    type: "field_dropdown",
                    name: "direction",
                    options: [
                        [ "left", "left" ],
                        [ "right", "right" ],
                    ]
                }
            ]
        }
    }

    static get pickupAction()
    {
        return {
            message0: "Pick up",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get dropAction()
    {
        return {
            message0: "Drop",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.COMMAND_COLOR,
        }
    }

    static get ifFlow()
    {
        return {
            message0: "If %1 then %2 else %3",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                {
                    type: "input_value",
                    name: "statement",
                    check: [ "flow_statement" ],
                },
                {
                    type: "input_statement",
                    name: "true_action",
                },
                {
                    type: "input_statement",
                    name: "false_action",
                }
            ]
        }
    }

    static get whileFlow()
    {
        return {
            message0: "While %1 do %2",
            previousStatement: null,
            nextStatement: null,
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                {
                    type: "input_value",
                    name: "statement",
                    check: [ "flow_statement" ],
                },
                {
                    type: "input_statement",
                    name: "action",
                }
            ]
        }
    }

    static get flowObjectDropdown()
    {
        return {
            type: "field_dropdown",
            name: "flow_statement_object",
            options: [
                [ "tile in front", "tile_in_front" ],
                [ "current tile", "current_tile" ],
            ]
        }
    }

    static get flowObjectStateDropdown()
    {
        return {
            type: "field_dropdown",
            name: "flow_statement_object_state",
            options: [
                [ "passable", "passable" ],
                [ "a box", "is_box" ],
                [ "a button", "is_button" ],
            ]
        }
    }

    static get flowStatementStateEquals()
    {
        return {
            message0: "%1 is %2",
            colour: SylveonBlocks.FLOW_COLOR,
            args0: [
                SylveonBlocks.flowObjectDropdown,
                SylveonBlocks.flowObjectStateDropdown,
            ],
            output: "flow_statement"
        }
    }
}

export default SylveonBlocks;