import React, {
    Component
} from 'react';
import LevelGrid from './game-grid/LevelGrid';

export class Home extends Component {
    static displayName = Home.name;

    // Mock Data
    static puzzle1InitialJSON = '{"character":{"directionCharacter":0,"directionCharacterString":"North","tile":{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":1,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":2,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":3,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":8,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"}],"puzzleWidth":3,"puzzleHeight":3,"puzzleLevel":1}';
    static puzzle2InitialJSON = '{"character":{"directionCharacter":3,"directionCharacterString":"West","tile":{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":2,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":8,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":3,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"iD":6,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"isOpen":false,"iD":8,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":9,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"iD":10,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"},{"iD":11,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"},{"iD":12,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":13,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":14,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":15,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}],"puzzleWidth":4,"puzzleHeight":4,"puzzleLevel":2}';
    static puzzle1ResultJSON = `
        { "levelNumber": 1, "solved": true, "lines": 1337, "states": 
            [
                {"character":{"directionCharacter":3,"directionCharacterString":"West","tile":{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":1,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":2,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":3,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":8,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"}],"puzzleWidth":3,"puzzleHeight":3,"puzzleLevel":1},
                {"character":{"directionCharacter":3,"directionCharacterString":"West","tile":{"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":1,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":2,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":3,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":8,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"}],"puzzleWidth":3,"puzzleHeight":3,"puzzleLevel":1},
                {"character":{"directionCharacter":2,"directionCharacterString":"South","tile":{"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":1,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":2,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":3,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":8,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"}],"puzzleWidth":3,"puzzleHeight":3,"puzzleLevel":1},
                {"character":{"directionCharacter":1,"directionCharacterString":"East","tile":{"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":1,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":2,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":3,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":6,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":8,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"}],"puzzleWidth":3,"puzzleHeight":3,"puzzleLevel":1}
            ]
        }`;
    static puzzle2ResultJSON = `
        {  "levelNumber": 2, "solved": true, "lines": 420691337, "states": 
            [
                {"character":{"directionCharacter":2,"directionCharacterString":"South","tile":{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}},"puzzleTiles":[{"iD":0,"state":4,"stateString":"End","movable":0,"movableString":"None"},{"iD":1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":2,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"door":{"isOpen":false,"iD":8,"state":0,"stateString":"Door","movable":0,"movableString":"None"},"iD":3,"state":3,"stateString":"Button","movable":0,"movableString":"None"},{"isOpen":false,"iD":4,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":5,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"iD":6,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":7,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"isOpen":false,"iD":8,"state":0,"stateString":"Door","movable":0,"movableString":"None"},{"iD":9,"state":1,"stateString":"Wall","movable":0,"movableString":"None"},{"iD":10,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"},{"iD":11,"state":2,"stateString":"Empty","movable":1,"movableString":"Box"},{"iD":12,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":13,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":14,"state":2,"stateString":"Empty","movable":0,"movableString":"None"},{"iD":15,"state":2,"stateString":"Empty","movable":0,"movableString":"None"}],"puzzleWidth":4,"puzzleHeight":4,"puzzleLevel":2}
            ]
        }`;

    constructor(props) {
        super(props);
        this.onUpdateButtonClicked = this.onUpdateButtonClicked.bind(this);
        this.state = {
            puzzleState: JSON.parse(Home.puzzle1InitialJSON),
            puzzleCompleted: false,
        }
    }

    onUpdateButtonClicked() {
        this.onReceiveUpdateJson(Home.puzzle1ResultJSON);
    }

    onReceiveUpdateJson(updateJson) {
        let puzzleStateUpdate = JSON.parse(updateJson);
        let puzzleFinalState = puzzleStateUpdate.states[puzzleStateUpdate.states.length - 1];

        this.setState({
            puzzleState: puzzleFinalState,
            puzzleCompleted: puzzleStateUpdate.solved
        });
    }

    render() {
        return ( <
            div >
            <
            div style = {
                {
                    'width': '300px'
                }
            } >
            <
            LevelGrid puzzle = {
                this.state.puzzleState
            }
            isComplete = {
                this.state.puzzleCompleted
            }
            /> <
            /div> <
            hr / >
            <
            div style = {
                {
                    'width': '300px'
                }
            } >
            <
            LevelGrid puzzle = {
                JSON.parse(Home.puzzle2InitialJSON)
            }
            isComplete = {
                false
            }
            /> <
            /div> <
            button onClick = {
                this.onUpdateButtonClicked
            } > Update! < /button> <
            /div>
        );
    }
}