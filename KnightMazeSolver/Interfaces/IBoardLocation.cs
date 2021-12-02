using System.Collections.Generic;

namespace KnightMazeSolver
{
    /// <summary>
    /// Represents a location on the board  
    /// </summary>
    public interface IBoardLocation
    {
        /// <summary>
        /// The one based X index 
        /// </summary>
        byte X { get; set; }

        /// <summary>
        /// The one based Y index 
        /// </summary>
        byte Y { get; set; }

        /// <summary>
        /// Initializes an IBoardLocation with the specified X and Y indexes
        /// </summary>
        /// <param name="x">The one based X index</param>
        /// <param name="y">The one based Y index</param>
        void Initialize(byte x, byte y);

        /// <summary>
        /// Tests another board location for equality 
        /// </summary>
        /// <param name="boardLocation">The location to compare to the current one</param>
        /// <returns>True if the locations are equal</returns>
        bool Equals(IBoardLocation boardLocation);
    }
}