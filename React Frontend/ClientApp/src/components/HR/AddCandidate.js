
import React from "react";
import "../../css/HR.css";

export function AddCandidate(props) {
    function checkenter(event){
        if(event.key === "Enter"){
            addCandidate(event)
        }
    }
    async function addCandidate(event){
        let credentials = event.target.parentNode.parentNode
        var name = credentials.querySelector("#username").value;
        fetch("api/HR/candidate", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: localStorage.getItem("token")
            },
            body: JSON.stringify(name)
        })
        .then(status)
        .then(() => document.querySelector("#loginerror").innerHTML = name + " is toegevoegd.")
        .catch(error => {
            document.querySelector("#loginerror").innerHTML = "Oeps! " + error;
        })
    }
    function translateErrorStatusCodeToString(statusCode){
        if(statusCode===400){
            return "Vul eerst een naam in."
        } else if(statusCode===401){
            return "De sessie is verlopen. Log opnieuw in."
        } else {
            return "Er is iets mis gegaan. Probeer het later opnieuw."
        }
    }
    function status(response){
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve("OK")
            } else {
                reject(translateErrorStatusCodeToString(response.status))
            }
        })
    }
    return (
        <article className = "singleBlock">
            <div className="login">Voeg kandidaat toe</div>
            <br/>
            <div>
                <div className="credentials">
                    <div>Naam kandidaat: <input placeholder="Naam" type="string" id="username" onKeyPress={checkenter}/></div>
                </div>
                <button id="loginbutton" onClick={addCandidate}>Voeg toe</button>
                <div className="error" id="loginerror"> </div>
            </div>
        </article>
    );
}
