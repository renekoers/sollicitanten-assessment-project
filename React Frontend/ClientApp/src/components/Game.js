import { Header } from "./Header";
import { Statement } from "./Statement";
import React, { useState, useEffect } from "react";
import LevelGrid from "./game-grid/LevelGrid";
import { SkipButton } from "./SkipButton";
import SylveonBlocks from "../blockly/SylveonBlocks";

export const Game = props => {
	let _currentStateTimeoutID = null;

	const STATE_CHANGE_ANIMATION_INTERVAL_TIME = 1000;

	const [gameOver, setGameOver] = useState(false);
	const [level, setLevel] = useState(null);
	const [solved, setSolved] = useState(false);
	const [levelNumber, setLevelNumber] = useState(1);
	const [totalLevels, setTotalLevels] = useState(0);

	useEffect(() => {
		getTotalLevelAmount();
		if (props.match.params.level) {
			getLevel(props.match.params.level);
		} else {
			getLevel(1);
		}
	}, []);

	const getTotalLevelAmount = async () => {
		await fetch("api/session/totalAmountLevels")
			.then(response => response.json())
			.then(data => {
				setTotalLevels(data);
			});
	};

	const getLevel = async level => {
		await fetch("api/session/retrieveLevel/" + level, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				setLevel(data);
				setLevelNumber(data.puzzleLevel);
				SylveonBlocks.clearWorkspace();
			});

		await fetch("api/session/levelIsSolved?levelNumber=" + level, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				setSolved(data);
			});
	};
	const pauseLevel = async () => {
		await fetch("api/session/pauseLevel", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			},
			body: JSON.stringify(levelNumber)
		});
	};
	const nextLevel = async () => {
		if (levelNumber !== totalLevels) {
			await pauseLevel();
			getLevel(level.puzzleLevel + 1);
		}
	};
	const previousLevel = async () => {
		if (levelNumber !== 1) {
			await pauseLevel();
			getLevel(level.puzzleLevel - 1);
		}
	};

	const onReceiveStatementTree = async statementTree => {
		const sessionId = localStorage.getItem("sessionID");
		const levelSolutionResponse = await fetch(
			"api/statement/" + levelNumber,
			{
				method: "POST",
				headers: {
					"content-type": "application/json",
					Authorization: sessionId
				},
				body: JSON.stringify(statementTree)
			}
		);
		const levelSolution = await levelSolutionResponse.json();
		updateGridFromLevelSolution(levelSolution);
	};

	/**
	 * @param {*} levelSolution The LevelSolution as returned by the API (See: BackEnd.Api.SubmitSolution(int, int, Statement[]))
	 */
	const updateGridFromLevelSolution = levelSolution => {
		if (_currentStateTimeoutID !== null)
			clearTimeout(_currentStateTimeoutID);
		_updateGridFromLevelSolutionAtStateIndex(levelSolution, 0);
	};

	const _updateGridFromLevelSolutionAtStateIndex = (
		levelSolution,
		currentStateIndex
	) => {
		const isFinalState =
			currentStateIndex === levelSolution.states.length - 1;
		const solved = isFinalState && levelSolution.solved;
		const currentState = levelSolution.states[currentStateIndex];

		setSolved(solved);
		setLevel(currentState);

		if (!isFinalState) {
			_currentStateTimeoutID = setTimeout(
				() =>
					_updateGridFromLevelSolutionAtStateIndex(
						levelSolution,
						currentStateIndex + 1
					),
				STATE_CHANGE_ANIMATION_INTERVAL_TIME
			);
		} // Set to null to indicate the sequence has been completed (and avoid potential conflicts with other timeouts).
		else _currentStateTimeoutID = null;
	};

	/**
	 * @returns JSX Levelgrid depending on the current levelnumber
	 */
	const renderLevelGrid = () => {
		let levelGrid;
		if (level !== null) {
			levelGrid = <LevelGrid puzzle={level} isComplete={solved} />;
		}
		return levelGrid;
	};

	return (
		<div>
			<Header hasTimer={true} />
			<div>
				<div style={{ width: "50%", float: "left" }}>
					<Statement
						levelNumber={levelNumber}
						onRunCode={onReceiveStatementTree.bind(this)}
					/>
				</div>
				<div style={{ width: "50%", float: "right" }}>
					{renderLevelGrid()}
					<SkipButton
						onClickPrevious={previousLevel.bind(this)}
						onClickNext={nextLevel.bind(this)}
						disabledPrevious={levelNumber === 1}
						lastLevel={levelNumber === totalLevels}
					/>
				</div>
			</div>
		</div>
	);
};
