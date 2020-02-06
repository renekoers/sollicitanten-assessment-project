import Blockly from "blockly";

class SylveonBlocks
{
    static get COMMAND_COLOR() { return 0; }
    static get FLOW_COLOR() { return 225; }

    static registerBlocks()
    {
        Blockly.Blocks["move_forward"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.moveForwardAction);
            }
        };
        Blockly.Blocks["rotate"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.rotateAction);
            }
        };
        Blockly.Blocks["pickup"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.pickupAction);
            }
        };
        Blockly.Blocks["drop"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.dropAction);
            }
        };
        Blockly.Blocks["if_then"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.ifFlow);
            }
        };
        Blockly.Blocks["while_do"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.whileFlow);
            }
        };
        Blockly.Blocks["state_equals"] = {
            init: function() {
                this.jsonInit(SylveonBlocks.flowStatementStateEquals);
            }
        }
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
                        [ "right", "rgiht" ],
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
                    name: "action",
                },
                {
                    type: "input_statement",
                    name: "action",
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