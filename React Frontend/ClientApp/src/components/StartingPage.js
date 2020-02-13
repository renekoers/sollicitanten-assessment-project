import React, { useState, useEffect } from "react";
import { Jumbotron, Button, Container } from "reactstrap";
import "../css/StartingPage.css";
import { Redirect } from "react-router-dom";

export const StartingPage = () => {
	const [sessionStarted, setSessionStatus] = useState(false);
	const [name, setName] = useState(null)
	useEffect(() => {getCandidate()},[])

	const getCandidate = async () => {
		if(IsSessionIdAvailable()){
			if(await isAlreadyStarted()){
				getCandidateName();
				setSessionStatus(true);
			} else if(await isSessionIDValid()){
				getCandidateName();
			} else {
				getNewCandidate();
			}
		} else {
			getNewCandidate();
		}
	}
	const getNewCandidate = async() => {
		await fetch("api/session/candidate")
			.then(checkStatus)
			.then(data => {
				setName(data.name);
				localStorage.setItem("sessionID", data.id)
			})
	}
	const getCandidateName = async() => {
		if(name !== null){
			return;
		}
		await fetch("api/session/candidate/"+localStorage.getItem("sessionID"))
			.then(checkStatus)
			.then(data => {
				setName(data.name);
			})
	}

	// const startSession = async () => {
	// 	if (!IsSessionIdAvailable()) {
	// 		await getNewCandidate();
	// 	} else if (!(await isSessionIDValid())) {
	// 		await getNewCandidate();
	// 	}
	// 	await startGameSession();
	// };

	const IsSessionIdAvailable = () => {
		const id = localStorage.getItem("sessionID");
		return id !== null;
	};

	const startSession = async () => {
		await fetch("api/session/startsession", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => setSessionStatus(response.status===200));
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
	}

	const gameSessionRedirect = () => {
		if (sessionStarted) {
			return <Redirect to="/gamesession" />;
		}
	};
	const checkStatus = response => {
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve(response.json())
            } else {
                reject(response)
            }
        })
	}
	if(name){
		return (
			<div>
				{gameSessionRedirect()}
				<Jumbotron fluid>
					<Container fluid>
						<h1 className="display-4">Welkom, {name}!</h1>
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
	} else {
		return <div> Er is geen sessie beschikbaar op dit moment. </div>
	}
};
