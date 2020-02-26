import React from "react";
import { LevelGrid } from "./LevelGrid";
import { SkipButton } from "./SkipButton";

export const Gamegrid = props => {
	const getLevel = props.getLevel;

	const renderLevelGrid = () => {
		let levelGrid;
		if (props.level !== null) {
			levelGrid = (
				<LevelGrid puzzle={props.level} isComplete={props.isSolved} />
			);
		}
		return levelGrid;
	};

	const pauseLevel = async () => {
		await fetch("api/session/pauseLevel", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			},
			body: JSON.stringify(props.levelNumber)
		});
	};

	const nextLevel = async () => {
		if (props.levelNumber <= props.totalLevelAmount) {
			await pauseLevel();
		}
		if (props.levelNumber !== props.totalLevelAmount) {
			getLevel(props.level.puzzleLevel + 1);
		}
	};

	const previousLevel = async () => {
		if (props.levelNumber !== 1) {
			await pauseLevel();
			getLevel(props.level.puzzleLevel - 1);
		}
	};

	return (
		<div>
			{renderLevelGrid()}
			<SkipButton
				onClickPrevious={previousLevel.bind(this)}
				onClickNext={nextLevel.bind(this)}
				disabledPrevious={props.levelNumber === 1}
				lastLevel={props.levelNumber === props.totalLevelAmount}
				running={props.areStatementsRunning}
			/>
		</div>
	);
};
