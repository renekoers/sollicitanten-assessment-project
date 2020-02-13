import React, { useState, useEffect } from "react";
import "../../css/HR.css";
import { Jumbotron, Container } from "reactstrap";
import { Statistics } from "./Statistics";
import { StatisticsButtons } from "./StatisticsButtons";

export function Results(props) {
	const [name, setName] = useState(null);
	const [firstID, setFirstID] = useState(1);
	const [lastID, setLastID] = useState(props.lastID);
	const [id, setID] = useState(props.id);
	useEffect(() => {
		if(id===0){
			getLastID();
		}
		props.onSeen(id)
		fetch("api/session/candidate/"+id)
		.then(status)
		.then(data => {setName(data.name)})
	},[id])
	
	async function getLastID(){
		fetch("api/statistics/lastFinished", {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
		.then(status)
		.then(data => {
			setLastID(data)
			setID(data)
		})
	}

	async function getPreviousID(){
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		await fetch("api/statistics/previousFinished?ID="+id, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
		.then(status)
		.then(setID)
        .catch(error => {
            if(error === 404){
				setFirstID(id);
			} else {
            	props.onInvalidSession(translateErrorStatusCodeToString(error))
            }
        })
	}
	async function getNextID(){
		document.querySelector("#loginerror").innerHTML = " ";
		document.querySelector(".popupButton").style["display"] = "none";
		await fetch("api/statistics/nextFinished?ID="+id, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: localStorage.getItem("token")
			}
		})
		.then(status)
		.then(setID)
        .catch(error => {
            if(error===404){
				setLastID(id);
			} else {
				props.onInvalidSession(translateErrorStatusCodeToString(error))
            }
        })
	}
    function status(response){
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve(response.json())
            } else {
                reject(response.status);
            }
        })
	}
    function translateErrorStatusCodeToString(statusCode){
        if(statusCode===401){
            return "De sessie is verlopen. Log opnieuw in."
        } else {
            return "Er is iets mis gegaan. Probeer het later opnieuw."
        }
	}
	if(id===0){
		return <div/>
	}
    return (
		<div>
			<Jumbotron fluid>
				<Container fluid>
					<h1 className="display-3">Statistieken {name}</h1>
					<Statistics key={id} id={id} />
					<StatisticsButtons disabledPrevious={id===firstID} disabledNext={id===lastID} onClickPrevious={getPreviousID} onClickNext={getNextID}  />
				</Container>
			</Jumbotron>
		</div>
    );
}
