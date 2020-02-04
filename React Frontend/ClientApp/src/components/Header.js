import React, { Component } from "react";
import { Timer } from "./Timer";
import "../css/Header.css";

export class Header extends Component {
	constructor(props) {
		super(props);
		this.state = {};
	}

	render() {
		return (
			<div>
				<div className="header-container">
					<div className="header-content">
						<Timer key="timer" />
					</div>
				</div>
			</div>
		);
	}
}
