﻿import { Header } from './Header';
import { Statement } from './Statement';
import React, { Component } from 'react';
import LevelGrid from './game-grid/LevelGrid';
import {SkipButton} from './SkipButton';

export class Game extends Component {
    _currentStateTimeoutID = null;

    static get STATE_CHANGE_ANIMATION_INTERVAL_TIME() //Closest thing we have to a constant class member in JS.
    {
        return 1000;
    }

    constructor(props) {
        super(props);
        this.state = {
            gameOver: false, level: null, solved: false, levelNumber: 1, totalLevels: 0
        }
    }

    async componentDidMount() {
        await this.getSessionID();
        this.getTotalAmountLevels();
        this.getLevel(1);
    }

    async getSessionID() {
            await fetch('api/session/startsession')
            .then(response => response.json())
            .then(data => {
                localStorage.setItem('sessionID', data);
            }
        )
    }
    async getTotalAmountLevels(){
        await fetch('api/session/totalAmountLevels')
        .then(response => response.json())
        .then(data => {
            this.setState({totalLevels: data});
        }
    )
    }

    async getLevel(level) {
        await fetch('api/session/retrieveLevel?levelNumber=' + level, {
            method: "GET",
            headers: {
                "Content-Type": "application/json", "Authorization": localStorage.getItem("sessionID")
            }
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ level: data, levelNumber: data.puzzleLevel })
            })
    }
    async pauseLevel() {
        await fetch("api/session/pauseLevel", {
          method: "POST",
          headers: {
            "Content-Type": "application/json", "Authorization": localStorage.getItem("sessionID")
          },
          body: JSON.stringify(this.state.levelNumber)
        })
    }
    async nextLevel(){
        if(this.state.levelNumber !== this.state.totalLevels){
            await this.pauseLevel()
            this.getLevel(this.state.level.puzzleLevel+1)
        }
    }
    async previousLevel(){
        if(this.state.levelNumber !== 1){
            await this.pauseLevel()
            this.getLevel(this.state.level.puzzleLevel-1)
        }
    }

    /**
     * @param {*} levelSolution The LevelSolution as returned by the API (See: BackEnd.Api.SubmitSolution(int, int, Statement[]))
     */
    updateGridFromLevelSolution(levelSolution)
    {
        if(this._currentStateTimeoutID !== null)
            clearTimeout(this._currentStateTimeoutID);
        this._updateGridFromLevelSolutionAtStateIndex(levelSolution, 0);
    }

    _updateGridFromLevelSolutionAtStateIndex(levelSolution, currentStateIndex)
    {
        const isFinalState = (currentStateIndex === (levelSolution.states.length - 1));
        const solved = isFinalState && levelSolution.solved;
        const currentState = levelSolution.states[currentStateIndex];

        this.setState({
            solved: solved,
            level: currentState,
        });

        if(!isFinalState)
        {
            this._currentStateTimeoutID = setTimeout(
                () => this._updateGridFromLevelSolutionAtStateIndex(levelSolution, currentStateIndex + 1), 
                Game.STATE_CHANGE_ANIMATION_INTERVAL_TIME
            );
        }
        else // Set to null to indicate the sequence has been completed (and avoid potential conflicts with other timeouts).
            this._currentStateTimeoutID = null;
    }

    render() {
        let levelGrid;
        if (this.state.level !== null) {
            levelGrid = <LevelGrid puzzle={this.state.level} isComplete={this.state.solved}/>
        }
        return ( 
            <div>
                <Header />
                <div>
                    <div style={{ 'width': '50%', 'float': 'left' }}>
                        <Statement />
                    </div>
                    <div style={{ 'width': '50%', 'float': 'right' }}>
                        {levelGrid}
                        <SkipButton name="Previous" onClick={this.previousLevel.bind(this)} disabled={this.state.levelNumber===1}/>
                        <SkipButton name="Next" onClick={this.nextLevel.bind(this)} disabled={this.state.levelNumber===this.state.totalLevels}/>
                    </div>
                </div>
            </div>
        );
    }
}

