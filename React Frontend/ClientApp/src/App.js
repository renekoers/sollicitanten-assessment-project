import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Game } from './components/Game';
import { Statistics } from './components/Statistics';

import './css/custom.css'

export default class App extends Component {
    static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/gamesession' component={Game} />
                <Route path='/statistics' component={Statistics} />
            </Layout>
        );
    }
}
