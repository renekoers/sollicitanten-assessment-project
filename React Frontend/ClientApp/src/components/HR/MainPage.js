import React, { useState, useEffect } from "react";
import { Jumbotron, Button, Container } from "reactstrap";
import { Login } from "./Login";

export function MainPage() {
	const [token, setToken] = useState(localStorage.getItem("token"));
	const [isValid, setIsValid] = useState(false);
	useEffect(() => {
		if(token !== null){
			fetch("api/credentials/validate", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: token
				}
			})
			.then(response => {
				setIsValid((response.status === 200))
			})
		}
	},[token]);
	
    function addToken(newToken){
        localStorage.setItem("token",newToken);
        setToken(newToken)
    }
   if(isValid){
    	console.log(token);
		return (
			<div>
				<Jumbotron fluid>
					<Container fluid>
						<h1 className="display-3">Statistieken</h1>
						<p>Jeej token!!!!</p>
					</Container>
				</Jumbotron>
			</div>
		);
   } else {
		return (
			<Login onLogin={addToken}/>
		);
	}
};
