using System;

using System.Collections.Generic;

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

    public class Move : EqualityComparer<Move>, IMove
    {
        public IBoardLocation StartingLocation { get; internal set; }
        public IBoardLocation EndingLocation { get; internal set; }

        public Move(IBoardLocation startingLocation, IBoardLocation endingLocation)
        {
            if (startingLocation == null)
            {
                throw new ArgumentNullException("Starting location must not be null");
            }

            if (endingLocation == null)
            {
                throw new ArgumentNullException("Ending location must not be null");
            }

            StartingLocation = startingLocation;
            EndingLocation = endingLocation;
        }
        public bool Equals(IMove move)
        {
            return move != null && StartingLocation.Equals(move.StartingLocation) && EndingLocation.Equals(move.EndingLocation);
        }

        public override bool Equals(Move x, Move y)
        {
            if (x == null)
            {
                return (y == null);
            }
            else if (y == null)
            {
                return false;
            }

            return x.StartingLocation.Equals(y.StartingLocation) && x.EndingLocation.Equals(y.EndingLocation);
        }

        public override int GetHashCode(Move move)
        {
            // Starting X = XXXXXXXX
            // Starting Y = YYYYYYYY
            // Ending X = WWWWWWWW
            // Ending Y = ZZZZZZZZ
            uint hCode = move.StartingLocation.X;  // hCode = 000000000000000000000000XXXXXXXX

            hCode = (uint)(hCode << 8);            // hCode = 0000000000000000XXXXXXXX00000000
            hCode |= move.StartingLocation.Y;      // hCode = 0000000000000000XXXXXXXXYYYYYYYY
            hCode = (uint)(hCode << 8);            // hCode = 00000000XXXXXXXXYYYYYYYY00000000
            hCode |= move.EndingLocation.X;        // hCode = 00000000XXXXXXXXYYYYYYYYWWWWWWWW
            hCode = (uint)(hCode << 8);            // hCode = XXXXXXXXYYYYYYYYWWWWWWWW00000000
            hCode |= move.EndingLocation.Y;        // hCode = XXXXXXXXYYYYYYYYWWWWWWWWZZZZZZZZ

            return hCode.GetHashCode();
        }

        public override string ToString()
        { 
            return $"{BoardLocation.ToString(StartingLocation)} => {BoardLocation.ToString(EndingLocation)}";
        }

        public static string ToString(IMove move)
        {
            return move == null ? "null" : $"{BoardLocation.ToString(move.StartingLocation)} => {BoardLocation.ToString(move.EndingLocation)}";
        }
    }
}
