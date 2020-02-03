import React, { Component } from "react";
import {Link} from 'react-router-dom';
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
        this.props.onClickNext();
    }
    render() {
        let classNames;
        if(this.props.disabledPrevious){
            classNames=["skipButton", "disabled"].join(' ');
        } else {
            classNames="skipButton";
        }
        let nextButton;
        if(this.props.lastLevel){
            nextButton = <Link to={'/overview'}> <button className="skipButton" onClick={ this.nextButtonClicked }> Overview</button></Link>;
        } else {
            nextButton = <button className="skipButton" onClick={ this.nextButtonClicked }> Next level</button>;
        }
        return (
            <div>
                <button disabled={this.props.disabledPrevious} className={classNames} onClick={ this.previousButtonClicked }> Previous level</button>
                {nextButton}
            </div>
        );
  }
}