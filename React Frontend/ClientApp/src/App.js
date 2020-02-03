import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Game } from './components/Game';
import { Overview } from './components/overview/Overview';

import './css/custom.css'

export default class App extends Component {
    static displayName = App.name;

    render () {
        return (
            <Layout>
                <Route exact path='/' component={Game} />
                <Route exact path='/gamesession/:level' component={Game} />
                <Route exact path='/gamesession' component={Game} />
                <Route path='/overview' component={Overview} />
            </Layout>
        );
    }
}
