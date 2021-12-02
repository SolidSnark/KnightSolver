using System;
using System.Collections.Generic;

namespace KnightMazeSolver
{
    public interface IKnight
    {
        /// <summary>
        /// A list of valid moves from the current board position
        /// </summary>
        List<IBoardLocation> ValidMoves { get; }

        /// <summary>
        /// The knight's current position on the board
        /// </summary>
        IBoardLocation BoardLocation { get; }

        /// <summary>
        /// Initializes the knight object and sets the location to the starting location
        /// </summary>
        /// <param name="board">The board to associate with the knight</param>
        void Initialize(IBoard board);

        /// <summary>
        /// The knight to a valid target location
        /// </summary>
        /// <param name="targetLocation">The new location for the knight</param>
        /// <returns>A move object with the starting and ending locations of the knight</returns>
        IMove Move(IBoardLocation targetLocation);
    }
}