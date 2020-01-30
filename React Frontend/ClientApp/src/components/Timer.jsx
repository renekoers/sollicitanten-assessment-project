import React, { Component } from "react";
//import '../styles/Timer.css';

export class Timer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      time: 0
    };
  }

  componentDidMount() {
    this.interval = setInterval(() => this.updateTime(), 1000);
  }

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  async updateTime() {
    let milliseconds = 0;

    await fetch("api/session/remainingtime", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(localStorage.getItem("sessionID"))
    })
      .then(response => response.json())
      .then(data => {
        milliseconds = data;
      });
    this.setState({ time: milliseconds });
  }

  formatTime() {
    let milliseconds = this.state.time;
    let minutes = Math.floor(milliseconds / 60000);
    let seconds = Math.floor(milliseconds / 1000) % 60;
    let formattedtime = minutes + ":" + seconds;
    return formattedtime;
  }

  render() {
    return (
      <div>
        <div className="timer-container">
          <div className="timer">{this.formatTime()}</div>
        </div>
      </div>
    );
  }
}
