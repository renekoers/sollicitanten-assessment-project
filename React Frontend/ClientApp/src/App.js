import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Game } from "./components/Game";
import { Overview } from "./components/overview/Overview";
import { StartingPage } from "./components/StartingPage";
import { EndPage } from "./components/EndPage";
import { MainPage as HR} from "./components/HR/MainPage"; 
import { Login } from "./components/HR/Login"; 
import { Tutorial } from "./components/Tutorial";

import "./css/custom.css";

export default class App extends Component {
	static displayName = App.name;

	render() {
		return (
			<Layout>
				<Route exact path="/" component={StartingPage} />
				<Route path="/tutorialsession" component={Tutorial} />
				<Route exact path="/gamesession/:level" component={Game} />
				<Route exact path="/gamesession" component={Game} />
				<Route path="/overview" component={Overview} />
				<Route path="/results" component={EndPage} />
				<Route exact path="/HR" component={HR} />
				<Route path="/HR/login" component={Login} />
			</Layout>
		);
	}
}
