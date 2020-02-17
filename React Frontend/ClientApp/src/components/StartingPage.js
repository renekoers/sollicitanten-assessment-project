import React, { useState, useEffect } from "react";
import {
	Jumbotron,
	Button,
	Container,
	Form,
	FormGroup,
	Label,
	Input
} from "reactstrap";
import "../css/StartingPage.css";
import { Redirect } from "react-router-dom";

export const StartingPage = () => {
	const [isTutorialSessionStarted, setTutorialSessionStatus] = useState(
		false
	);
	const [sessionStarted, setSessionStatus] = useState(false);
	const [name, setName] = useState(null);

	useEffect(() => {
		if (localStorage.getItem("sessionID") === null) {
			fetch("api/session/candidate/getUnstartedCandidates")
				.then(response => response.json())
				.then(data => {
					console.log(data);
					setAvailableCandidates(data);
					let candidateOptions = data.map((candidate, index) => {
						return (
							<option value={candidate.id} key={index}>
								{candidate.name}
							</option>
						);
					});
					setAvailableCandidateOptions(candidateOptions);
				});
		} else {
			getCandidate();
		}
	}, []);

	const startTutorialSession = async () => {
		await fetch("api/session/startTutorialSession")
			.then(response => {
				if (response.ok) {
					setTutorialSessionStatus(true);
				} else {
					throw Error("Error whilst fetching session, no OK");
				}
			})
			.catch(error => {
				console.log(error);
			});
	};

	const getCandidate = async () => {
		if (IsSessionIdAvailable()) {
			if (await isAlreadyStarted()) {
				getCandidateName();
				setSessionStatus(true);
			} else if (await isSessionIDValid()) {
				getCandidateName();
			} else {
				getNewCandidate();
			}
		} else {
			getNewCandidate();
		}
	};

	const getNewCandidate = async () => {
		await fetch("api/session/candidate")
			.then(checkStatus)
			.then(data => {
				console.log(data);
				setName(data.name);
				localStorage.setItem("sessionID", data.id);
			});
	};

	const getCandidateName = async () => {
		if (name !== null) {
			return;
		}
		await fetch(
			"api/session/candidate/" + localStorage.getItem("sessionID")
		)
			.then(checkStatus)
			.then(data => {
				setName(data.name);
			});
	};

	const IsSessionIdAvailable = () => {
		const id = localStorage.getItem("sessionID");
		return id !== null;
	};

	const tutorialSessionRedirect = () => {
		if (isTutorialSessionStarted) {
			return <Redirect to="/tutorialsession" />;
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
	const isAlreadyStarted = async () => {
		let isStarted;
		await fetch("api/session/isStarted", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				isStarted = data;
			});
		return isStarted;
	};

	const gameSessionRedirect = () => {
		if (sessionStarted) {
			return <Redirect to="/gamesession" />;
		}
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

	// Select a candidate page
	const [dropdownValue, setDropdownValue] = useState();
	const [availableCandidateOptions, setAvailableCandidateOptions] = useState(
		null
	);
	const [availableCandidates, setAvailableCandidates] = useState(null);

	// useEffect(() => {}, []);

	// const toggleCandidateDropdown = () => {
	// 	setCandidateDropdownOpen(!candidateDropdownOpen);
	// };

	const handleCandidateSelection = () => {
		localStorage.setItem("sessionID", dropdownValue);
		const selectedCandidateName = availableCandidates.map(candidate => {
			if (candidate.id === dropdownValue) {
				return candidate.name;
			}
		});
		setName(selectedCandidateName);
	};

	if (name) {
		return (
			<div>
				{tutorialSessionRedirect()}
				{gameSessionRedirect()}
				<Jumbotron fluid>
					<Container fluid>
						<h1 className="display-4">Welkom, {name}!</h1>
						<p className="lead">
							Wanneer je op start drukt volgt een tutorial van het
							sollicitatieprogrammeerspel. De bedoeling is om jouw
							karakter op de finish te laten eindigen aan de hand
							van commando's die jij het geeft. Hierbij is het van
							belang dit met zo min mogelijk commando's te doen.
						</p>
						<p className="lead">Veel geluk pik.</p>
					</Container>
				</Jumbotron>
				<Button
					color="primary start-button"
					onClick={() => startTutorialSession()}
				>
					Start tutorial
				</Button>
			</div>
		);
	} else {
		return (
			<div>
				<Jumbotron fluid>
					<Container fluid>
						<h1 className="display-2">Kies kandidaat</h1>
						<Form>
							<FormGroup>
								<Label for="selectCandidate">
									Selecteer kandidaat
								</Label>
								<Input
									type="select"
									name="select"
									id="candidateSelect"
									onChange={e =>
										setDropdownValue(e.target.value)
									}
									value={dropdownValue}
								>
									{availableCandidateOptions}
								</Input>
							</FormGroup>
						</Form>

						<Button
							color="primary start-button"
							onClick={() => handleCandidateSelection()}
						>
							Selecteer kandidaat
						</Button>
					</Container>
				</Jumbotron>
			</div>
		);
	}
};
