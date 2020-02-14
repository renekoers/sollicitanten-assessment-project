import React, { useState, useEffect } from "react";
import { AddCandidate } from "./AddCandidate";
import { Results } from "./Results";
import { Redirect, Link } from "react-router-dom";
import "../../css/HR.css";

export function MainPage() {
	const [token, setToken] = useState(localStorage.getItem("token"));
	const [isValid, setIsValid] = useState(null);
	const [lastCheck, setLastCheck] = useState(null);
	const [newFinishedIDs, setNewFinishedIds] = useState([]);
	const [atResults, setAtResults] = useState(false);
	const [lastID, setLastID] = useState(0);
	useEffect(() => {
		if(localStorage.getItem("token") !== token){
			setToken(localStorage.getItem("token"));
		}
	},[localStorage.getItem("token")])

	useEffect(() => {
		async function validate(){
			fetch("api/HR/validate", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: token
				}
			})
			.then(response => {
				if(response.status===200){
					setIsValid(true)
				} else {
					setIsValid(false)
					setToken(null)
				}
			})
		}
		if(token !== null){
			validate();
		} else {
			setIsValid(false)
		}
	},[token]);
	useEffect(() => getNewFinished(),[])

    useEffect(() => {
        document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		getLastID();
	},[token])
	
	async function getLastID(){
		fetch("api/statistics/lastFinished", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: token
			}
		})
		.then(status)
		.then(setLastID)
	}
	
	useEffect(() => {
		const id = setInterval(() => getNewFinished(), 20000);
		return () => clearInterval(id);
	},[lastCheck])

	
	async function getNewFinished(){
		let time = "";
		if(lastCheck!==null){
			time = "?time=" + lastCheck
		}
		await fetch("api/statistics/newFinished" + time, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
		.then(status)
		.then(data => {
			let maxID = lastID;
			if(data.length>0){
				var arrayID = newFinishedIDs;
				data.forEach(id => {
					arrayID.push(id);
					if(id>maxID){
						maxID=id;
					}
				});
				setNewFinishedIds(arrayID);
			}
			if(maxID>lastID){
				setLastID(maxID)
			}
			setLastCheck(Date.now())
		})
	}	
    function status(response){
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve(response.json())
            } else {
                reject(response.status)
            }
        })
	}
	function removeIDFromNewFinishedIDs(id){
		let arrayIDs = newFinishedIDs;
		const index = arrayIDs.indexOf(id);
		if (index > -1) {
			arrayIDs.splice(index, 1);
		}
		setNewFinishedIds(arrayIDs)
	}
	
	function toLogin(error){
		document.querySelector("#loginerror").innerHTML = "Oeps! " + error;
		document.querySelector(".popupButton").style.removeProperty("display");
	}
	function changePage(){
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		setAtResults(!atResults);
	}
	function getPage(){
		if(isValid){
			if(atResults){
				let id=lastID;
				if(newFinishedIDs.length>0){
					id=newFinishedIDs[0];
				}
				return (
					<div>
						<button className="upperButton" onClick={changePage}>Voeg kandidaat toe</button>
						<Results onInvalidSession={toLogin} id={id} lastID={lastID} onSeen={removeIDFromNewFinishedIDs}/>
					</div>
				);
			} else {
				return (
					<div>
						{newResults()}<button className="upperButton" onClick={changePage}>Naar resultaten </button>
						<AddCandidate onInvalidSession={toLogin}/>
					</div>
				);
			}
		}
	}
	function newResults(){
		if(isValid){
			if(newFinishedIDs.length>0){
				return <div className="badge">{newFinishedIDs.length}</div>
			} else {
				return <div/>
			}
		}
	}
	return (
		<div>
			{ getPage()}
			<div className="error" id="loginerror"> </div>
			<Link to="HR/login" className="popupButton"><button id="loginbutton">Naar login</button></Link>
			{!isValid && isValid!==null && token===null &&<Redirect to="/HR/login"/>}
		</div>
	);
};
