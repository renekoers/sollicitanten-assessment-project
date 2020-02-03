import React, { useState, useEffect } from 'react';
import {Link} from 'react-router-dom';
import '../../css/overview.css';

export function LevelOverview(props) {
  const [levelNumber, setLevelNumber] = useState(0);
  const [solved, setSolved] = useState(false);
  const [numberOfLines, setNumberOfLines] = useState(0);
  const [par, setPar] = useState(0);
  const [classNames, setClassNames] = useState("levelOverview")
  useEffect(() => {
      setLevelNumber(props.info.levelNumber);
      setSolved(props.info.solved);
      if(props.info.solved){
          setNumberOfLines(props.info.lines);
          setPar(props.info.par)
          setClassNames(["levelOverview", "solved"].join(' '));
      } else {
        setClassNames(["levelOverview", "unsolved"].join(' '));
      }
  },[]);
  return (
    <div className={classNames}>
        <Link to={'/gamesession/'+levelNumber} className={classNames}> {levelNumber}: {solved ? "Solved in " + numberOfLines + " lines. Par is " + par : "Not solved"} </Link>
    </div>
  );
}