import React, {
    Component
} from 'react';

export class Tile extends Component {

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
                    return String.fromCharCode(65 + this.props.tile.id); //65 = 'A'
                }
                case "Wall":
                    return '#';
                case "Empty":
                    return '.';
                case "Button":
                    return String.fromCharCode(97 + this.props.tile.door.id); //97 = 'a'
                case "End":
                    return '!';
                default:
                    throw "Invalid state: " + this.props.tile.stateString;

        }
    }

    render() {
        return ( <
            div className = "tile" > {
                this.convertTypeToASCII()
            } <
            /div>
        );
    }
}
export default Tile;