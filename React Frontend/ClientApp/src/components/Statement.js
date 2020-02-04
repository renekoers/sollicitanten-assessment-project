import React, { Component } from "react";
import "../css/statement.css";

export class Statement extends Component {
  static displayName = Statement.name;

  constructor(props) {
    super(props);
    this.currentButtons = [];
    this.currentStatements = [
      "MoveForward",
      "RotateLeft",
      "RotateRight",
      "PickUp",
      "Drop",
      "--While--",
      "--If--",
      "--Else--",
      "--End--",
      "CheckFront",
      "CanMoveForward",
      "Button",
      "Box"
    ];
    this.counter = 0;
    this.state = { currentButtons: this.currentButtons, counter: 0 };
    this.showOffline = this.showOffline.bind(this);
    this.deleteButton = this.deleteButton.bind(this);
  }

  showOffline(e) {
    this.currentButtons.push(
      <button
        id={e.target.id + this.counter}
        onClick={this.deleteButton}
        key={e.target.id + this.counter}
      >
        {e.target.id}
      </button>
    );
    this.counter = this.counter + 1;
    this.setState({
      currentButtons: this.currentButtons,
      counter: this.counter
    });
  }

  deleteButton(e) {
    this.currentButtons = this.currentButtons.filter(
      el => el.props.id !== e.target.id
    );
    this.setState({
      currentButtons: this.currentButtons
    });
  }

  render() {
    return (
      <div>
        <div id="wrapper">
          <div id="input">
            {this.currentStatements.map((stmt, index) => (
              <button id={stmt} onClick={this.showOffline} key={index}>
                {stmt}
              </button>
            ))}
          </div>
          <div id="output">{this.currentButtons}</div>
          <button
            style={{ backgroundColor: "Pink" }}
            onClick={this.commitStatements}
          >
            {" "}
            Run puzzle!{" "}
          </button>
        </div>
      </div>
    );
  }
}
