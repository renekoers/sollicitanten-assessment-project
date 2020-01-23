using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public interface IState
    {
        /// <summary>
        /// Returns game's character object
        /// </summary>
        ICharacter Character { get; }

        /// <summary>
        /// Returns list that contains tiles with information from top-left of grid on pos 0, 
        /// bottom-right tile on last pos.
        /// </summary>
        List<Tile> PuzzleTiles { get; }

        /// <summary>
        /// Returns int with puzzle's width
        /// </summary>
        int PuzzleWidth { get; }

        /// <summary>
        /// Returns in with puzzle's height
        /// </summary>
        int PuzzleHeight { get;  }
    }
}
