import React, { useState } from "react";
import { Jumbotron, Button, Container } from "reactstrap";
import "../css/StartingPage.css";
import { Redirect } from "react-router-dom";

export const StartingPage = () => {
	const [sessionStarted, setSessionStatus] = useState(false);

	const startSession = async () => {
		if (!IsSessionIdAvailable()) {
			await makeNewSession();
		} else if (!(await isSessionValid())) {
			await makeNewSession();
		}
		setSessionStatus(true);
	};

	const IsSessionIdAvailable = () => {
		const id = localStorage.getItem("sessionID");
		if (id === null) {
			return false;
		} else {
			return true;
		}
	};

	const makeNewSession = async () => {
		await fetch("api/session/startsession")
			.then(response => response.json())
			.then(data => {
				localStorage.setItem("sessionID", data);
			});
	};

	const isSessionValid = async () => {
		let sessionExists;
		await fetch("api/session/sessionValidation", {
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

	const gameSessionRedirect = () => {
		if (sessionStarted) {
			return <Redirect to="/gamesession" />;
		}
	};

	return (
		<div>
			{gameSessionRedirect()}
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-4">Welkom, Kandidaat!</h1>
					<p className="lead">
						Wanneer je op start drukt volgt een tutorial van het
						sollicitatieprogrammeerspel. De bedoeling is om jouw
						karakter op de finish te laten eindigen aan de hand van
						commando's die jij het geeft. Hierbij is het van belang
						dit met zo min mogelijk commando's te doen.
					</p>
					<p className="lead">Veel geluk pik.</p>
				</Container>
			</Jumbotron>
			<Button color="primary start-button" onClick={() => startSession()}>
				Start Spel
			</Button>
		</div>
	);
};
