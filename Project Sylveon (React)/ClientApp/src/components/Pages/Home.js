import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Hello, world!</h1>
        <p>Dit is een corona vrije zone</p>
        <p>Hier komt trouwens ook iets van een start pagina o.i.d.</p>
      </div>
    );
  }
}
