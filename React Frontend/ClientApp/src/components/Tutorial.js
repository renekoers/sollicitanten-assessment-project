import React, { useState, useEffect } from "react";
import { Header } from "./Header";
import { LevelGrid } from "./game-grid/LevelGrid";
import { Statement } from "./Statement";
import { Button, ButtonToolbar, ButtonGroup } from "reactstrap";
import { Redirect } from "react-router-dom";
import { HelpPopUp } from "./HelpPopUp";

export const Tutorial = () => {
	let _currentStateTimeoutID = null;

	const STATE_CHANGE_ANIMATION_INTERVAL_TIME = 1000;

	const tutorialSessionID = 0;
	const [level, setLevel] = useState(null);
	const [isSolved, setSolved] = useState(false);
	const [isLiveSessionStarted, setLiveSessionStarted] = useState(false);

	useEffect(() => {
		getTutorialLevel();
	}, []);

	const getTutorialLevel = async () => {
		await fetch("api/session/retrieveLevel/0", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: tutorialSessionID
			}
		})
			.then(response => response.json())
			.then(data => {
				setLevel(data);
			});

		await fetch("api/session/levelIsSolved/0", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: tutorialSessionID
			}
		})
			.then(response => response.json())
			.then(data => {
				setSolved(data);
			});
	};

	const onReceiveTutorialStatementTree = async statementTree => {
		const levelSolutionResponse = await fetch("api/statement/0", {
			method: "POST",
			headers: {
				"content-type": "application/json",
				Authorization: tutorialSessionID
			},
			body: JSON.stringify(statementTree)
		});
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
			levelGrid = <LevelGrid puzzle={level} isComplete={isSolved} />;
		}
		return levelGrid;
	};

	const IsSessionIdAvailable = () => {
		const id = localStorage.getItem("sessionID");
		if (id === null) {
			return false;
		} else {
			return true;
		}
	};

	const isSessionIDValid = async () => {
		let sessionExists;
		await fetch("api/session/sessionIDValidation", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				sessionExists = data;
			});
		return sessionExists;
	};

	const startLiveSession = async () => {
		if (!IsSessionIdAvailable()) {
			await makeNewSession();
		} else if (!(await isSessionIDValid())) {
			await makeNewSession();
		}
		await startSession();
	};

	const makeNewSession = async () => {
		await getNewCandidate();
	};

	const getNewCandidate = async () => {
		await fetch("api/session/candidate")
			.then(checkStatus)
			.then(data => {
				localStorage.setItem("sessionID", data.id);
			});
	};

	const checkStatus = response => {
		return new Promise(function(resolve, reject) {
			if (response.status === 200) {
				resolve(response.json());
			} else {
				reject(response);
			}
		});
	};

	const startSession = async () => {
		await fetch("api/session/startsession", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		}).then(response => setLiveSessionStarted(response.status === 200));
	};

	const redirectToLiveGameSession = () => {
		if (isLiveSessionStarted) {
			return <Redirect to="/gamesession" />;
		}
	};

	return (
		<div>
			{redirectToLiveGameSession()}
			<Header hasTimer={false} />
			<div>
				<div style={{ width: "50%", float: "left" }}>
					<Statement
						levelNumber={1}
						onRunCode={onReceiveTutorialStatementTree.bind(this)}
					/>
					<ButtonToolbar>
						<ButtonGroup>
							<Button
								color="primary"
								onClick={() => startLiveSession()}
							>
								Eindig Tutorial
							</Button>
						</ButtonGroup>
						<ButtonGroup>
							<HelpPopUp buttonLabel={"?"} />
						</ButtonGroup>
					</ButtonToolbar>
				</div>

				<div style={{ width: "50%", float: "right" }}>
					{renderLevelGrid()}
				</div>
			</div>
		</div>
	);
};
