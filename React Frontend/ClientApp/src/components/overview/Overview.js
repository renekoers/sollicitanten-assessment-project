import React, { useState, useEffect } from 'react';
import {LevelOverview} from './LevelOverview';
import { Header } from '../Header';

export function Overview() {
  const [levels, setLevels] = useState([]);
  useEffect(() => {
      getOverview();
  },[]);

  async function getOverview(){
    await fetch('api/session/getOverview', {
        method: "GET",
        headers: {
            "Content-Type": "application/json", "Authorization": localStorage.getItem("sessionID")
        }
    })
        .then(response => response.json())
        .then(data => {
            setLevels(data.levels);
        })
}
  return (
    <div>
        <Header />
        {levels.map((key,index) => (
            <LevelOverview key={index} info={key} />
        ))} 
    </div>
  );
}