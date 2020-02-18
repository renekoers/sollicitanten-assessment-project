import React, { useState, useEffect } from "react";
import {
	Jumbotron,
	Button,
	Container,
	Form,
	FormGroup,
	Label,
	Input,
	Spinner
} from "reactstrap";
import { Redirect } from "react-router-dom";
import "../css/StartingPage.css";

export const StartingPage = () => {
	const [isTutorialSessionStarted, setTutorialSessionStatus] = useState(
		false
	);
	const [sessionStarted, setSessionStatus] = useState(false);
	const [name, setName] = useState(null);
	const [
		isEligibleCandidateAvailable,
		setIsEligibleCandidateAvailable
	] = useState(false);
	const [isNoCandidateChosen, setIsNoCandidateChosen] = useState(false);
	const [dropdownValue, setDropdownValue] = useState("kieskandidaat");
	const [availableCandidateOptions, setAvailableCandidateOptions] = useState(
		null
	);
	const [availableCandidates, setAvailableCandidates] = useState(null);

	useEffect(() => {
		if (IsSessionIdAvailable()) {
			checkSessionIDForEligibleCandidate();
			// Check if SessionID is still available after checking its candidate.
			if (IsSessionIdAvailable()) {
				renderCandidateWelcomePage();
			} else {
				renderChooseCandidatePage();
			}
		} else {
			getAllUnstartedCandidates();
			renderChooseCandidatePage();
		}
	}, []);

	const renderCandidateWelcomePage = () => {
		setIsEligibleCandidateAvailable(true);
		setIsNoCandidateChosen(false);
	};

	const renderChooseCandidatePage = () => {
		setIsNoCandidateChosen(true);
		setIsEligibleCandidateAvailable(false);
	};

	const getAllUnstartedCandidates = () => {
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
	};

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

	const checkSessionIDForEligibleCandidate = async () => {
		const sessionID = localStorage.getItem("sessionID");
		if (await hasCandidateNotYetStarted(sessionID)) {
			setName(await getCandidateName(sessionID));
		} else if (await isCandidateStillActive(sessionID)) {
			setSessionStatus(true);
		} else {
			localStorage.removeItem("sessionID");
		}
	};

	// const getNewCandidate = async () => {
	// 	await fetch("api/session/candidate")
	// 		.then(checkStatus)
	// 		.then(data => {
	// 			console.log(data);
	// 			setName(data.name);
	// 			localStorage.setItem("sessionID", data.id);
	// 		});
	// };

	const getCandidateName = async () => {
		let candidateName;
		await fetch(
			"api/session/candidate/" + localStorage.getItem("sessionID")
		)
			.then(checkStatus)
			.then(data => {
				candidateName = data.name;
			});
		return candidateName;
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
		await fetch("api/session/sessionIDEligibleation", {
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

	const hasCandidateNotYetStarted = async () => {
		let hasNotYetStarted;
		await fetch("api/session/hasNotYetStarted", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				hasNotYetStarted = data;
			});
		return hasNotYetStarted;
	};

	const isCandidateStillActive = async () => {
		let isStillActive;
		await fetch("api/session/isActive", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				isStillActive = data;
			});
		return isStillActive;
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

	const handleCandidateSelection = () => {
		localStorage.setItem("sessionID", dropdownValue);
		const selectedCandidateName = availableCandidates.map(candidate => {
			console.log("Dropdown:" + dropdownValue);
			console.log("Candidate:" + candidate);
			if (candidate.id === dropdownValue) {
				console.log("Got value:" + dropdownValue);
				return candidate.name;
			}
		});
		setName(selectedCandidateName);
		setIsEligibleCandidateAvailable(true);
	};

	if (isEligibleCandidateAvailable && name !== null) {
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
	} else if (isNoCandidateChosen) {
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
									onChange={e => {
										console.log(e.target.value);
										setDropdownValue(e.target.value);
									}}
									value={dropdownValue}
								>
									<option disabled value="kieskandidaat">
										Kies kandidaat
									</option>
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
	} else {
		return (
			<div>
				Loading...
				<Spinner
					style={{
						width: "3rem",
						height: "3rem"
					}}
					type="grow"
					color="success"
				/>
			</div>
		);
	}
};
