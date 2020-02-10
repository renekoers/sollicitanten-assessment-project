import React, { useState, useEffect } from "react";
import { Login } from "./Login";
import { AddCandidate } from "./AddCandidate";

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
		return (
			<AddCandidate/>
		);
   } else {
		return (
			<Login onLogin={addToken}/>
		);
	}
};
