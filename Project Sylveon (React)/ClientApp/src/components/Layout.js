import React, { Component } from 'react';
import { Container } from 'reactstrap';

export class Layout extends Component {
  static displayName = Layout.name;

    // Layout is de layout van de pagina ten alle tijden. 
    // Dus misschien dat we dit aan willen passen afhankelijk van startscherm en het daadwerkelijk spelen van het spel.
    // Of weghalen

  render () {
    return (
      <div>
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
