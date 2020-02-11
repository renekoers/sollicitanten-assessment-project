import React, { useState } from "react";
import "../../css/HR.css";
import { Jumbotron, Container } from "reactstrap";
import { Statistics } from "./Statistics";

export function Results(props) {
    const [id, setID] = useState(this.props.id);
    
    return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-3">Statistieken</h1>
					<Statistics id={localStorage.getItem("sessionID")} />
				</Container>
			</Jumbotron>
		</div>
    );
}
