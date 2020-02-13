import React from "react";
import { Jumbotron, Container } from "reactstrap";

export const EndPage = props => {
	// const [sessionID, setSessionID] = useState(-1);

	// useEffect(() => {
	// 	const ID = localStorage.getItem("sessionID");
	// 	if (!ID === null) {
	// 		console.log(ID);
	// 		setSessionID(ID);
	// 	} else {
	// 		return (
	// 			<div>
	// 				<p>Invalid session ID</p>
	// 			</div>
	// 		);
	// 	}
	// }, []);

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
