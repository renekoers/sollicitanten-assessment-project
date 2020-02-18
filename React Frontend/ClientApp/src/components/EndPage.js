import React from "react";
import { Jumbotron, Container } from "reactstrap";

export const EndPage = props => {

	return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-3">Statistieken</h1>
					<p>Je bent klaar!</p>
				</Container>
			</Jumbotron>
		</div>
	);
};
