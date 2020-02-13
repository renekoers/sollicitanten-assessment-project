import React, { Component } from "react";
import { Timer } from "./Timer";
import "../css/Header.css";

export const Header = props => {
	const timer = () => {
		if (props.hasTimer) {
			return <Timer key="timer" />;
		} else {
			return;
		}
	};

	return (
		<div>
			<div className="header-container">
				<div className="header-content">{timer()}</div>
			</div>
		</div>
	);
};
