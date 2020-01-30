import React, { Component } from 'react';
import './LevelGrid.css';

export class Tile extends Component {
    constructor(props) {
        super(props);
        this.state = { tile: props.tile };
    }

    convertTypeToASCII() {
        switch (this.state.tile.movableString) {
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
        switch (this.state.tile.stateString) {
            case "Door":
                if (this.state.tile.isOpen) {
                    return '.';
                } else {
                    return String.fromCharCode(65 + this.state.tile.iD);
                }
            case "Wall":
                return '#';
            case "Empty":
                return '.';
            case "Button":
                return String.fromCharCode(97 + this.state.tile.door.iD);
            case "End":
                return '!';

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