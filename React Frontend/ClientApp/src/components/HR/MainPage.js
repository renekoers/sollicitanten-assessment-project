import React, { useState, useEffect } from "react";
import { AddCandidate } from "./AddCandidate";
import { Results } from "./Results";
import { Redirect, Link } from "react-router-dom";

export function MainPage() {
	const [token, setToken] = useState(localStorage.getItem("token"));
	const [isValid, setIsValid] = useState(false);
	const [lastCheck, setLastCheck] = useState(null);
	const [newFinishedIDs, setNewFinishedIds] = useState([]);
	const [atResults, setAtResults] = useState(false);
	const [lastID, setLastID] = useState(0);

    useEffect(() => {
        document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		fetch("api/statistics/lastFinished", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: token
			}
		})
		.then(status)
		.then(setLastID)
	},[])
	useEffect(() => {
		if(localStorage.getItem("token") !== token){
			setToken(localStorage.getItem("token"));
		}
	}, [localStorage.getItem("token")])
	
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
	
	useEffect(() => {
		const id = setInterval(getNewFinished(), 20000);
		return () => clearInterval(id);
	},[lastCheck])


	async function getNewFinished(){
		let time = "";
		if(lastCheck!==null){
			time = "?time=" + lastCheck
		}
		await fetch("api/HR/newFinished" + time, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
		.then(status)
		.then(data => {
			let maxID = lastID;
			var arrayID = newFinishedIDs;
			data.forEach(id => {
				arrayID.push(id);
				if(id>maxID){
					maxID=id;
				}
			});
			if(maxID>lastID){
				setLastID(maxID)
			}
			setNewFinishedIds(arrayID);
			setLastCheck(Date.now())
		})
	}
	
	function toLogin(error){
		document.querySelector("#loginerror").innerHTML = "Oeps! " + error;
		document.querySelector(".popupButton").style.removeProperty("display");
	}
    function status(response){
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve("OK")
            } else {
                reject(response.status)
            }
        })
	}
	function changePage(){
		setAtResults(!atResults);
	}
	function getPage(){
		if(isValid){
			if(atResults){
				return (
					<div>
						<button onClick={changePage}>Voeg kandidaat toe</button>
						<Results onInvalidSession={toLogin} lastID={lastID}/>
					</div>
				);
			} else {
				return (
					<div>
						<button onClick={changePage}>Naar resultaten {getButtonResults()}</button>
						<AddCandidate onInvalidSession={toLogin}/>
					</div>
				);
			}
		}
		if(token===null){
			return <Redirect to="/HR/login"/>;
		}
	}
	function getButtonResults(){
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
			{getPage()}
			<div className="error" id="loginerror"> </div>
			<Link to="HR/login" className="popupButton"><button id="loginbutton">Naar login</button></Link>
		</div>
	);
};
