import React, { Component } from "react";
import {Redirect} from 'react-router-dom';
import '../styles/SkipButton.css'

export class SkipButton extends Component {
    constructor(props){
        super(props)
        this.previousButtonClicked = this.previousButtonClicked.bind(this);
        this.nextButtonClicked = this.nextButtonClicked.bind(this);
    }
    previousButtonClicked(){
        this.props.onClickPrevious();
    }
    nextButtonClicked(){
        if(this.props.lastLevel){
            return  <Redirect  to="/overview" />;
        } else {
            this.props.onClickNext();
        }
    }
    render() {
        let classNames;
        if(this.props.disabledPrevious){
            classNames=["skipButton", "disabled"].join(' ');
        } else {
            classNames="skipButton";
        }
        return (
            <div>
                <button disabled={this.props.disabledPrevious} className={classNames} onClick={ this.previousButtonClicked }> Previous level</button>
                <button className="skipButton" onClick={ this.nextButtonClicked }> {this.props.lastLevel ? "Go to overview" : "Next level"}</button>
            </div>
        );
  }
}