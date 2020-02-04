﻿import { Header } from "./Header";
import { Statement } from "./Statement";
import React, { Component } from "react";
import LevelGrid from "./game-grid/LevelGrid";
import { SkipButton } from "./SkipButton";

export class Game extends Component {
  constructor(props) {
    super(props);
    this.state = {
      gameOver: false,
      level: null,
      solved: false,
      levelNumber: 1,
      totalLevels: 0
    };
  }

  async componentDidMount() {
    this.getTotalAmountLevels();
    this.getLevel(1);
  }

  async getTotalAmountLevels() {
    await fetch("api/session/totalAmountLevels")
      .then(response => response.json())
      .then(data => {
        this.setState({ totalLevels: data });
      });
  }

  async getLevel(level) {
    await fetch("api/session/retrieveLevel?levelNumber=" + level, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: localStorage.getItem("sessionID")
      }
    })
      .then(response => response.json())
      .then(data => {
        this.setState({ level: data, levelNumber: data.puzzleLevel });
      });
  }
  async pauseLevel() {
    await fetch("api/session/pauseLevel", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: localStorage.getItem("sessionID")
      },
      body: JSON.stringify(this.state.levelNumber)
    });
  }
  async nextLevel() {
    if (this.state.levelNumber !== this.state.totalLevels) {
      await this.pauseLevel();
      this.getLevel(this.state.level.puzzleLevel + 1);
    }
  }
  async previousLevel() {
    if (this.state.levelNumber !== 1) {
      await this.pauseLevel();
      this.getLevel(this.state.level.puzzleLevel - 1);
    }
  }

  render() {
    let levelGrid;
    if (this.state.level !== null) {
      levelGrid = (
        <LevelGrid puzzle={this.state.level} isComplete={this.state.solved} />
      );
    }
    return (
      <div>
        <Header />
        <div>
          <div style={{ width: "50%", float: "left" }}>
            <Statement />
          </div>
          <div style={{ width: "50%", float: "right" }}>
            {levelGrid}
            <SkipButton
              name="Previous"
              onClick={this.previousLevel.bind(this)}
              disabled={this.state.levelNumber === 1}
            />
            <SkipButton
              name="Next"
              onClick={this.nextLevel.bind(this)}
              disabled={this.state.levelNumber === this.state.totalLevels}
            />
          </div>
        </div>
      </div>
    );
  }
}
