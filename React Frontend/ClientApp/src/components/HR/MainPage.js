import React, { useState, useEffect } from "react";
import { Login } from "./Login";
import { AddCandidate } from "./AddCandidate";

export function MainPage() {
	const [token, setToken] = useState(localStorage.getItem("token"));
	const [isValid, setIsValid] = useState(false);
	useEffect(() => {
		if(token !== null){
			fetch("api/HR/validate", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: token
				}
			})
			.then(response => {
				setIsValid((response.status === 200))
			})
		} else {
			setIsValid(false)
		}
	},[token]);
	
    function addToken(newToken){
        localStorage.setItem("token",newToken);
        setToken(newToken)
	}
	function toLogin(){
		localStorage.setItem("token", null);
	}
   if(isValid){
		return (
			<AddCandidate onInvalidSession={toLogin}/>
		);
   } else {
		return (
			<Login onLogin={addToken}/>
		);
	}
};
