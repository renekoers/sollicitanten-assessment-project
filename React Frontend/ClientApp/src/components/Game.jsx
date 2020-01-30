import { Header } from './Header';
import { Statement } from './Statement';
import React, { Component } from 'react';
import LevelGrid from './game-grid/LevelGrid';

export class Game extends Component {
    constructor(props) {
        super(props);
        this.state = {
            gameOver: false, level: 0
        }
    }

    async componentDidMount() {
        await this.getSessionID();
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
    async getLevel(level) {
        await fetch('api/session/retrieveLevel?levelNumber=' + level, {
            method: "GET",
            headers: {
                "Content-Type": "application/json", "Authorization": localStorage.getItem("sessionID")
            }
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ level: data })
            })
    }

    render() {
        let levelGrid;
        if (this.state.level !== 0) {
            levelGrid = <LevelGrid puzzle={this.state.level}/>
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
                    </div>
                </div>
            </div>
        );
    }
}

