import React, { useState, useEffect } from 'react';
import {LevelOverview} from './LevelOverview';

export function Overview() {
  const [levels, setLevels] = useState(null);
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
        {levels.map((key,index) => (
            <LevelOverview key={index} info={key} />
        ))} 
    </div>
  );
}