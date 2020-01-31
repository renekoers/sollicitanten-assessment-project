import React, { Component } from "react";
//import '../styles/Timer.css';

export class Timer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      time: 1200000
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
    const id = localStorage.getItem("sessionID")
    await fetch("api/session/remainingtime", {
      method: "GET",
      headers: {
        "Content-Type": "application/json", "Authorization": id
      }
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

    if (minutes < 10) {
      minutes = "0" + minutes;
    }

    if (seconds < 10) {
      seconds = "0" + seconds;
    }
    return minutes + ":" + seconds;
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
