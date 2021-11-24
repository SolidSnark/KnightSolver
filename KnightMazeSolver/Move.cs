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
        IBoardLocation StartingLocation { get; set; }
        
        /// <summary>
        /// The knight's location after the move
        /// </summary>
        IBoardLocation EndingLocation { get; set; }        
    }

    public class Move : EqualityComparer<Move>, IMove
    {
        public IBoardLocation StartingLocation { get; set; }
        public IBoardLocation EndingLocation { get; set; }

        public Move(IBoardLocation startingLocation, IBoardLocation endingLocation)
        {
            StartingLocation = startingLocation;
            EndingLocation = endingLocation;
        }

        public override bool Equals(Move x, Move y)
        {
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
    }
}
