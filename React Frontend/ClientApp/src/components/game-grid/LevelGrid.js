import React, { Component } from 'react';
import Tile from './Tile';
import help from '../../img/help-icon.png';

export class LevelGrid extends Component {
    constructor(props) {
        super(props);
        var puzzleWidth = props.puzzle.puzzleWidth;
        var widthStr = "";
        for (var i = 0; i < puzzleWidth+2; i++) {
            widthStr += "auto ";
        }
        var allTiles = props.puzzle.puzzleTiles;
        this.placeCharacter(props.puzzle.character, allTiles);

        var startIndex = 0;
        var newTiles = [];
        this.addRowOfWalls(newTiles, puzzleWidth);
        while (startIndex < allTiles.length) {
            startIndex = this.addRowOfTiles(newTiles, allTiles, puzzleWidth, startIndex);
        }
        this.addRowOfWalls(newTiles, puzzleWidth);
        this.state = { width: widthStr, tiles: newTiles };

    }
    placeCharacter(character, allTiles) {
        allTiles[character.tile.id].movableString = "Character " + character.directionCharacterString;
    }
    showLegend() {
        document.getElementById("popupLegend").style.display = "block";
    }
    hideLegend() {
        document.getElementById("popupLegend").style.display = "none";
    }
    addRowOfWalls(tiles, width) {
        const extraWall = JSON.parse('{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}');
        for (var index = 0; index < width+2; index++) {
            tiles[tiles.length] = extraWall;
        }
    }
    addRowOfTiles(newTiles, originalTiles, width, startIndex) {
        const extraWall = JSON.parse('{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}');
        newTiles[newTiles.length] = extraWall;
        for (var index = 0; index < width; index++) {
            newTiles[newTiles.length] = originalTiles[startIndex+index];
        }
        newTiles[newTiles.length] = extraWall;
        return startIndex + width;
    }

    render() {
        return (
            <div>
                <img className="help" onClick={this.showLegend} src={help} alt="help"/>
                <div id="popupLegend" className="popup" onClick={this.hideLegend}>
                    <article className="singleBlock">
                        <div className="legend">Legend {<span className="close" onClick={this.hideLegend}>&times;</span>}</div>
                        <br />
                        <div>
                            <div> ! is the end of the level. </div>
                            <div> # is a wall. </div>
                            <div> {"^, <, _, > is the position of the character."} </div>
                            <div> * is a box. </div>
                            <div> An uppercase letter is a closed door. </div>
                            <div> A lowercase letter is a button. </div>
                        </div>
                    </article>

                </div>

                <div className="game-grid-container" style={{'display': 'grid', 'gridTemplateColumns': this.state.width}} >
                    {this.state.tiles.map((key,index) => (
                        <Tile key={index} tile={key} />
                    ))}                    
                    </div>
            </div>
        );
    }
}
export default LevelGrid;