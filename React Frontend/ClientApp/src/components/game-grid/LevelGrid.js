import React, { Component } from "react";
import Tile from "./Tile";
import "../../css/LevelGrid.css";

export class LevelGrid extends Component {
	state = {
		width: "",
		tiles: []
	};

	static getDerivedStateFromProps(props, currentState) {
		var puzzleWidth = props.puzzle.puzzleWidth;
		var widthStr = "";
		for (var i = 0; i < puzzleWidth + 2; i++) {
			widthStr += "auto ";
		}
		var allTiles = props.puzzle.puzzleTiles;
		LevelGrid.placeCharacter(props.puzzle.character, allTiles);

		var startIndex = 0;
		var newTiles = [];
		LevelGrid.addRowOfWalls(newTiles, puzzleWidth);
		while (startIndex < allTiles.length) {
			startIndex = LevelGrid.addRowOfTiles(
				newTiles,
				allTiles,
				puzzleWidth,
				startIndex
			);
		}
		LevelGrid.addRowOfWalls(newTiles, puzzleWidth);
		return {
			width: widthStr,
			tiles: newTiles
		};
	}

	static placeCharacter(character, allTiles) {
		allTiles[character.tile.id].movableString =
			"Character " + character.directionCharacterString;
	}

	static addRowOfWalls(tiles, width) {
		const extraWall = JSON.parse(
			'{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}'
		);
		for (var index = 0; index < width + 2; index++) {
			tiles[tiles.length] = extraWall;
		}
	}

	static addRowOfTiles(newTiles, originalTiles, width, startIndex) {
		const extraWall = JSON.parse(
			'{"id":-1,"state":1,"stateString":"Wall","movable":0,"movableString":"None"}'
		);
		newTiles[newTiles.length] = extraWall;
		for (var index = 0; index < width; index++) {
			newTiles[newTiles.length] = originalTiles[startIndex + index];
		}
		newTiles[newTiles.length] = extraWall;
		return startIndex + width;
	}

	render() {
		return (
			<div>
				<div
					className="game-grid-container"
					style={{
						display: "grid",
						gridTemplateColumns: this.state.width
					}}
				>
					{this.state.tiles.map((key, index) => (
						<Tile key={index} tile={key} />
					))}
				</div>
				<span>Level complete: {this.props.isComplete.toString()}</span>
			</div>
		);
	}
}
export default LevelGrid;
