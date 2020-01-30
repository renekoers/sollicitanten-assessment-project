import { Header } from './Header';
import { Statement } from './Statement';
import React, { Component } from 'react';

export class Game extends Component {
    constructor(props) {
        super(props);
        this.state = {
            gameOver: false,
        }
    }

    componentDidMount() {
        this.getSessionID();
    }

    async getSessionID() {
            await fetch('api/session/startsession')
            .then(response => response.json())
            .then(data => {
                localStorage.setItem('sessionID', data);
            }
    )}

    render() {
        return ( 
            <div>
                <Header />
                <Statement />
            </div>
        );
    }
}

