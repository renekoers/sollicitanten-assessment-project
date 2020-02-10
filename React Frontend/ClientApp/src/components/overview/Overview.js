import React, { useState, useEffect } from "react";
import { LevelOverview } from "./LevelOverview";
import { Header } from "../Header";
import { Button } from "reactstrap";
import { Redirect } from "react-router-dom";

export const Overview = () => {
	const [levels, setLevels] = useState([]);
	const [isFinished, setFinished] = useState(false);

	useEffect(() => {
		getOverview();
	}, []);

	const getOverview = async () => {
		await fetch("api/session/getOverview", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("sessionID")
			}
		})
			.then(response => response.json())
			.then(data => {
				setLevels(data.levels);
			});
	};

	const toggleFinishedState = () => {
		setFinished(true);
	};

	const redirectToAnalysationPage = () => {
		if (isFinished) {
			return <Redirect to="/results" />;
		}
	};

	return (
		<div>
			{redirectToAnalysationPage()}
			<Header />
			{levels.map((key, index) => (
				<LevelOverview key={index} info={key} />
			))}
			<Button
				color="primary"
				className="finalize-button"
				onClick={() => toggleFinishedState()}
			>
				Vermoord session
			</Button>
		</div>
	);
};
