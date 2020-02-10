import React, { useState, useEffect } from "react";
import { Jumbotron, Button, Container } from "reactstrap";

export function MainPage() {
    const [token, setToken] = useState(null);
    useEffect(() => {
        if(localStorage.getItem("token", token) == null){
            fetch("api/credentials/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify('{"username": "HR", "password": "pass"}')
              })
              .then(response => response.json())
              .then(token => {localStorage.setItem("token", token);
                setToken(token)});
        }
    },[]);
   useEffect(() => {
    if(localStorage.getItem("token", token) != null){
        fetch("api/credentials/test", {
            method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token", token)
			}
        })
        .then(response => response.json())
        .then(data => console.log(data))
    } else {
        fetch("api/credentials/test", {
            method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token", token)
			}
        })
        .then(response => response.json())
        .then(data => console.log(data))
    }
   },[])
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
