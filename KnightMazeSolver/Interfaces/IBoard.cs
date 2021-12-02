using System.Collections.Generic;

namespace KnightMazeSolver
{
    /// <summary>
    /// Represets a game board and contains data for the board state
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// The width of the board
        /// </summary>
        byte Width { get; }

        /// <summary>
        /// The height of the board
        /// </summary>
        byte Height { get; }

        /// <summary>
        /// The minimum width/height supported
        /// </summary>
        byte MinimumBoardSize { get; }

        /// <summary>
        /// The starting location for the maze and sets the board location to valid
        /// </summary>
        IBoardLocation StartingLocation { get; set; }

        /// <summary>
        /// The ending location for the maze and sets the board location to valid
        /// </summary>
        IBoardLocation EndingLocation { get; set; }

        /// <summary>
        /// A reference to the knight which contains its current location
        /// </summary>
        IKnight Knight { get; }

        /// <summary>
        /// The array of SquareColor containing the board data.  
        /// This allows board data to be retrieved and set
        /// It is indexed into by (X, Y)        
        /// </summary>
        /// <param name="x">The x coordinate of the location to access</param>
        /// <param name="y">The y coordinate of the location to access</param>
        /// <returns></returns>
        SquareColor this[byte x, byte y] { get; set; }

        /// <summary>
        /// The array of SquareColor containing the board data.  
        /// This allows board data to be retrieved and set
        /// </summary>
        /// <param name="location">The location of the square to access</param>
        /// <returns></returns>
        SquareColor this[IBoardLocation location] { get; set; }

        /// <summary>
        /// Initializes board with requested width and height and creates the 
        /// board data
        /// </summary>
        /// <param name="width">The requested width of the board. The minimum value is in the property MinimumBoardSize.</param>
        /// <param name="height">The requested height of the board. The minimum value is in the property MinimumBoardSize.</param>
        void Initialize(byte width, byte height);

        /// <summary>
        /// Initializes the board with an array of strings representing board rows.
        /// There must be at least MinimumBoardSize rows, at least MinimumBoardSize 
        /// characters wide and all must be the same length.
        /// The key is as follows:
        /// . = Void, invalid board square
        /// X - Valid board square
        /// S - Starting Location
        /// E - Ending Location
        /// </summary>
        /// <param name="rows">An array of strings representing board rows</param>
        void LoadStateData(string[] rows);

        /// <summary>
        /// Indicates if a location is within the bounds of the current board
        /// </summary>
        /// <param name="boardLocation">The location to test</param>
        /// <returns>True if the location is within the bounds of the current board</returns>
        bool IsSquareInBounds(IBoardLocation boardLocation);

        /// <summary>
        /// Indicates if a location is both within the bounds of the current board and a valid board square
        /// </summary>
        /// <param name="boardLocation">The location to test</param>
        /// <returns>True if the location is both within the bounds of the current board and a valid board square</returns>
        bool IsValidTargetSquare(IBoardLocation boardLocation);

        /// <summary>
        /// Validates the board for correctness and returns a list of error messages if it fails
        /// </summary>
        /// <param name="messages">A list of validation error messages</param>
        /// <returns>True if the board is valid</returns>
        bool ValidateBoard(out List<string> messages);
    }
}