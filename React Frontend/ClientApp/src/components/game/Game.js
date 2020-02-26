import React, { useState, useEffect, useCallback } from "react";
import { Redirect } from "react-router-dom";
import SylveonBlocks from "../../blockly/SylveonBlocks";
import { Header } from "./header/Header";
import { Statement } from "./Statement";
import { Gamegrid } from "./game-grid/GameGrid";

export const Game = props => {
	const [level, setLevel] = useState(null);
	const [levelNumber, setLevelNumber] = useState(1);
	const [totalLevelAmount, setTotalLevelAmount] = useState(0);
	const [isGamesessionFinished, setIsGamesessionFinished] = useState(false);
	const [isSolved, setIsSolved] = useState(false);
	const [areStatementsRunning, setAreStatementsRunning] = useState(false);

	const STATE_CHANGE_ANIMATION_INTERVAL_TIME = 1000;
	let currentStateTimeoutID = null;

	const getLevel = useCallback(
		async level => {
			await fetch("api/session/retrieveLevel/" + level, {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: localStorage.getItem("sessionID")
				}
			})
				.then(status)
				.then(data => {
					setLevel(data);
					setLevelNumber(data.puzzleLevel);
					SylveonBlocks.clearWorkspace();
				});

			await fetch("api/session/levelIsSolved/" + level, {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: localStorage.getItem("sessionID")
				}
			})
				.then(response => response.json())
				.then(data => {
					setIsSolved(data);
				});
		},
		[setLevel, setLevelNumber, setIsSolved]
	);

	useEffect(() => {
		setTotalLevelAmount(totalLevelAmount);
		if (props.match.params.level) {
			getLevel(props.match.params.level);
		} else {
			getLevel(1);
		}
	}, [getLevel, props.match.params.level, totalLevelAmount]);

	useEffect(() => {
		const getTotalLevelAmount = async () => {
			await fetch("api/session/totalAmountLevels")
				.then(response => response.json())
				.then(data => {
					setTotalLevelAmount(data);
				});
		};
		getTotalLevelAmount();
	}, []);

	const onReceiveStatementTree = async statementTree => {
		setAreStatementsRunning(true);
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
		if (currentStateTimeoutID !== null) clearTimeout(currentStateTimeoutID);
		updateGridFromLevelSolutionAtStateIndex(levelSolution, 0);
	};

	const status = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(response);
			}
		});
	};

	const updateGridFromLevelSolutionAtStateIndex = (
		levelSolution,
		currentStateIndex
	) => {
		const isFinalState =
			currentStateIndex === levelSolution.states.length - 1;
		const solved = isFinalState && levelSolution.solved;
		const currentState = levelSolution.states[currentStateIndex];

		setIsSolved(solved);
		setLevel(currentState);

		if (!isFinalState) {
			currentStateTimeoutID = setTimeout(
				() =>
					updateGridFromLevelSolutionAtStateIndex(
						levelSolution,
						currentStateIndex + 1
					),
				STATE_CHANGE_ANIMATION_INTERVAL_TIME
			);
		} // Set to null to indicate the sequence has been completed (and avoid potential conflicts with other timeouts).
		else {
			currentStateTimeoutID = null;
			setAreStatementsRunning(false);
		}
	};

	const sessionIsOverCallback = isSessionOver => {
		setIsGamesessionFinished(isSessionOver);
	};

	const redirectToEndPage = () => {
		if (isGamesessionFinished) {
			localStorage.removeItem("sessionID");
			return <Redirect to="/results" />;
		}
	};

	return (
		<div>
			{redirectToEndPage()}
			<Header
				hasTimer={true}
				sessionIsOverCallback={sessionIsOverCallback.bind(this)}
			/>
			<div>
				<div style={{ width: "50%", float: "left" }}>
					<Statement
						levelNumber={levelNumber}
						onRunCode={onReceiveStatementTree.bind(this)}
					/>
				</div>
				<div style={{ width: "50%", float: "right" }}>
					<Gamegrid
						level={level}
						levelNumber={levelNumber}
						totalLevelAmount={totalLevelAmount}
						isSolved={isSolved}
						getLevel={getLevel.bind(this)}
						areStatementsRunning={areStatementsRunning}
					/>
				</div>
			</div>
		</div>
	);
};
