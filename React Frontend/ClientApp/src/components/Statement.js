import React, { Component } from "react";
import "../css/statement.css";

export class Statement extends Component {
	static displayName = Statement.name;

  constructor(props) {
      super(props);
      this.currentButtons = [];
      this.currentSingleStatements = ["MoveForward", "RotateLeft", "RotateRight", "PickUp","Drop"];
      this.currentMultiStatements = ["--While--","--If--","--End--"];
      this.currentConditionalStatements = ["TileCurrent","TileFront"];
      this.currentChecks = ["Passable","Button","HasMovable"];
      this.counter = 0
      this.state = { currentButtons: this.currentButtons, counter: 0 };
      this.addButton = this.addButton.bind(this);
      this.deleteButton = this.deleteButton.bind(this);
  }

    addButton(e) {
        this.currentButtons.push(<button key={this.counter} id={e.target.id + this.counter} onClick={this.deleteButton}>{e.target.id}</button>);
        this.counter = this.counter + 1
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

    handleStatements = () => {
        var statements = [this.props.levelNumber.toString()];
        this.currentButtons.map((stmt) =>
            statements.push(stmt.props.children.replace(/-/ig,''))
            );
        this.props.onIncomingStatements(statements);
    }

  render() {
    return (
        <div>
            <div id="wrapper">
                <div id="input">
                    Single:
                    {this.currentSingleStatements.map((stmt) =>
                        <button key={stmt} id={stmt} onClick={this.addButton}>{stmt}</button>
                    )} 
                    Loop:
                    {this.currentMultiStatements.map((stmt) =>
                        <button key={stmt} id={stmt} onClick={this.addButton}>{stmt}</button>
                    )} 
                    Conditional:
                    {this.currentConditionalStatements.map((stmt) =>
                        <button key={stmt} id={stmt} onClick={this.addButton}>{stmt}</button>
                    )} 
                    Checks(?):
                    {this.currentChecks.map((stmt) =>
                        <button key={stmt} id={stmt} onClick={this.addButton}>{stmt}</button>
                    )} 
                </div>
                <div id="output">
                    {this.currentButtons}
                </div>
                <button style={{ backgroundColor: 'Pink' }} onClick={this.handleStatements}> Run puzzle! </button>
            </div>
      </div>
    );
  }
}