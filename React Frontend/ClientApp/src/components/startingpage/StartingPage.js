import React, { useState, useEffect } from "react";
import { Redirect } from "react-router-dom";
import "../../css/StartingPage.css";
import { Loadingscreen } from "../Loadingscreen";
import { WelcomeScreen } from "./WelcomeScreen";
import { CanditateSelectionPage } from "./CandidateSelectionPage";

export const StartingPage = () => {
	const [isTutorialSessionStarted, setTutorialSessionStatus] = useState(
		false
	);
	const [isCurrentSessionActive, setSessionStatus] = useState(false);
	const [isNoTokenAvailable, setIsNoTokenAvailable] = useState(false);
	const [name, setName] = useState(null);
	const [
		renderCandidateSelectionPage,
		setRenderCandidateSelectionPage
	] = useState(false);
	const [renderWelcomePage, setRenderWelcomePage] = useState(false);

	useEffect(() => {
		const sessionID = localStorage.getItem("sessionID");
		const HRToken = localStorage.getItem("token");

		const isSessionIdAvailable = () => {
			return sessionID !== null;
		};

		const isHRTokenAvailable = () => {
			return HRToken !== null;
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
		const checkStatusCandidate = async () => {
			let candidateStatus;
			await fetch("api/candidate/status", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: localStorage.getItem("sessionID")
				}
			})
			.then(checkStatus)
			.then(data => {
				candidateStatus = data
			});
			return candidateStatus;
		};

		const getCandidateName = async () => {
			let candidateName;
			await fetch("api/candidate/" + localStorage.getItem("sessionID"))
				.then(checkStatus)
				.then(data => {
					candidateName = data.name;
				});
			return candidateName;
		};

		const sessionIDHandling = async () => {
			var candidateStatus = checkStatusCandidate()
			if (isSessionIdAvailable() && candidateStatus && !candidateStatus.finished) {
				if (candidateStatus.started) {
					setSessionStatus(true);
				} else {
					setName(await getCandidateName(sessionID));
					setRenderWelcomePage(true);
				}
			} else {
				localStorage.removeItem("sessionID");
				if (isHRTokenAvailable()) {
					setRenderCandidateSelectionPage(true);
				} else {
					setIsNoTokenAvailable(true);
				}
			}
		};
		sessionIDHandling();
	}, [
		setName,
		setRenderWelcomePage,
		setRenderCandidateSelectionPage,
		setSessionStatus
	]);

	const tutorialSessionRedirect = () => {
		if (isTutorialSessionStarted) {
			return <Redirect to="/tutorialsession" />;
		}
	};

	const startTutorialSession = () => {
		setTutorialSessionStatus(true);
	};

	const selectedCandidateNameCallBack = name => {
		if (name !== null) {
			setName(name);
			setRenderWelcomePage(true);
		} else {
			return;
		}
	};

	const gameSessionRedirect = () => {
		if (isCurrentSessionActive) {
			return <Redirect to="/gamesession" />;
		}
	};

	const HRLoginRedirect = () => {
		if (isNoTokenAvailable) {
			return <Redirect to="/HR/login" />;
		}
	};

	if (renderWelcomePage) {
		return (
			<div>
				{tutorialSessionRedirect()}
				<WelcomeScreen
					name={name}
					startTutorialSession={startTutorialSession.bind(this)}
				/>
			</div>
		);
	} else if (renderCandidateSelectionPage) {
		return (
			<CanditateSelectionPage
				selectedCandidateNameCallBack={selectedCandidateNameCallBack.bind(
					this
				)}
			/>
		);
	} else if (isCurrentSessionActive) {
		return <div>{gameSessionRedirect()}</div>;
	} else if (isNoTokenAvailable) {
		return <div>{HRLoginRedirect()}</div>;
	} else {
		return (
			<div>
				<Loadingscreen />
			</div>
		);
	}
};
