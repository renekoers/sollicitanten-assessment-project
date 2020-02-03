import React, { useState } from "react";
import { Jumbotron, Button, Container } from "reactstrap";

export const StartingPage = props => {
  const [sessionStarted, setSessionStatus] = useState(false);

  const startSession = () => {
    setSessionStatus(true);
  };

  return (
    <div>
      <Jumbotron fluid>
        <Container fluid>
          <h1 className="display-3">Welkom Soggy Boy!</h1>
          <p className="lead">
            Lorem ipsum dolor sit amet, dictum sodales fringilla, taciti arcu
            odit etiam, elit blandit consequat lectus. Mi ultricies cras sit nam
            sem, nunc laoreet, id curabitur aliquam, sed leo pellentesque
            suscipit diam, proin incididunt sit. Vitae nulla et vitae.
            Suspendisse tristique ullamcorper, mattis in mi et faucibus.
            Adipiscing nam suscipit in mauris sed, velit soluta fermentum nam
            nunc ornare. Magna eu enim, metus at suscipit neque, est dui
            interdum, orci lectus sed, nascetur etiam.
          </p>
        </Container>
      </Jumbotron>
      <Button color="primary">Stard Spèl</Button>
    </div>
  );
};
