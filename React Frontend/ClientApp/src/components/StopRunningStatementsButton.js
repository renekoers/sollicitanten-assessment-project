import React, { Component } from "react";
import {Link} from 'react-router-dom';
import '../css/StopRunningStatementsButton.css';


export class StopRunningStatementsButton extends Component {
	constructor(props) {
		super(props);
        this.stopRunningButtonClicked = this.stopRunningButtonClicked.bind(this);
	}

	stopRunningButtonClicked() {
		this.props.stopRunningStatements();
	}
	render() {
		let running = this.props.running;
		
		return (
			<div>
				<button
					disabled={!running}
					className={"stopRunningStatements"}
					onClick={this.stopRunningButtonClicked}
				>
					Cancel execution
				</button>
			</div>
		);
	}
}
