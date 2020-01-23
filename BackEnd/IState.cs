using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public interface IState
    {
        /// <summary>
        /// Retrieve game's main character
        /// </summary>
        ICharacter Character { get; }
        /// <summary>
        /// Retrieves a list of the gamegrid, keeping in mind that the
        /// grid is rendered from top-left -> bottom-right
        /// </summary>
        List<Tile> PuzzleTiles { get; }
        int PuzzleWidth { get; }
        int PuzzleHeight { get;  }
    }
}
