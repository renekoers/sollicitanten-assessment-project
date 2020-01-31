import React, { Component } from 'react';

export class Tile extends Component {
    constructor(props) {
        super(props);
    }
    
    convertTypeToASCII() {
        switch (this.props.tile.movableString) {
            case "Box":
                return '*';
            case "Character North":
                return '^';
            case "Character East":
                return '>';
            case "Character South":
                return '_';
            case "Character West":
                return '<';
        }
        switch (this.props.tile.stateString) {
            case "Door":
                if (this.props.tile.isOpen) {
                    return '.';
                } else {
                    return String.fromCharCode(65 + this.state.tile.id);
                }
            case "Wall":
                return '#';
            case "Empty":
                return '.';
            case "Button":
                return String.fromCharCode(97 + this.state.tile.door.id);
            case "End":
                return '!';
            default:
                throw "Invalid state: " + this.state.tile.stateString;

        }
    }

    render() {
        return (
            <div className="tile">
                {this.convertTypeToASCII()}
            </div>
        );
    }
}
export default Tile;