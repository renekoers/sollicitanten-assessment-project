
import React from "react";
import "../../css/HR.css";

export function Login(props) {
    function checkenter(event){
        if(event.key === "Enter"){
            login(event)
        }
    }
    async function login(event){
        let credentials = event.target.parentNode.parentNode
        var username = credentials.querySelector("#username").value;
        var password = credentials.querySelector("#password").value;
        fetch("api/HR/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({"username":  username, "password": password })
        })
        .then(status)
        .then(token => props.onLogin(token))
        .catch(error => {
            document.querySelector("#loginerror").innerHTML = "Oeps! " + error;
        })
    }
    function translateErrorStatusCodeToString(statusCode){
        if(statusCode===400){
            return "Alle velden moeten ingevuld worden."
        } else if(statusCode===401){
            return "De combinatie van gebruikersnaam en wachtwoord is niet correct."
        } else {
            return "Er is iets mis gegaan. Probeer het later opnieuw."
        }
    }
    function status(response){
        return new Promise(function(resolve, reject){
            if(response.status === 200){
                resolve(response.json())
            } else {
                reject(translateErrorStatusCodeToString(response.status))
            }
        })
    }
    return (
        <article className = "singleBlock">
            <div className="login">Login</div>
            <br/>
            <div>
                <div className="credentials">
                    <div>Gebruikersnaam: <input placeholder="Gebruikersnaam" type="string" id="username" /></div>
                    <div>Wachtwoord: <input placeholder="Wachtwoord" type="password" id="password" className="password" onKeyPress={checkenter}/></div>
                </div>
                <button id="loginbutton" onClick={login}>Log in</button>
                <div className="error" id="loginerror"> </div>
            </div>
        </article>
    );
}
