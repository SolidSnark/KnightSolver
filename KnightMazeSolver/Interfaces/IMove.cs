namespace KnightMazeSolver
{
    /// <summary>
    /// Defines a move of the knight
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// The knight's location before the move
        /// </summary>
        IBoardLocation StartingLocation { get; }

        /// <summary>
        /// The knight's location after the move
        /// </summary>
        IBoardLocation EndingLocation { get; }

        /// <summary>
        /// Tests another move for equality 
        /// </summary>
        /// <param name="move">The move to compare to the current one</param>
        /// <returns>True if the moves are equal</returns>
        bool Equals(IMove move);
    }
}