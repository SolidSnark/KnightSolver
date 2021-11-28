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

    public class BoardLocation : EqualityComparer<BoardLocation>, IBoardLocation
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public BoardLocation(byte x, byte y)
        {
            Initialize(x, y);
        }

        public void Initialize(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(IBoardLocation boardLocation)
        {
            return boardLocation != null && X == boardLocation.X && Y == boardLocation.Y;
        }

        public override bool Equals(BoardLocation x, BoardLocation y)
        {
            if (x == null)
            {
                return (y == null);
            }
            else if (y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public override int GetHashCode(BoardLocation boardLocation)
        {          
            // X = XXXXXXXX
            // Y = YYYYYYYY
            ushort hCode = boardLocation.X; // hCode = 00000000XXXXXXXX
            
            hCode = (ushort)(hCode << 8);   // hCode = XXXXXXXX00000000
            hCode |= boardLocation.Y;       // hCode = XXXXXXXXYYYYYYYY
            
            return hCode.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(this);
        }

        public static string ToString(IBoardLocation boardLocation)
        {
            return boardLocation == null ? "null" : $"({boardLocation.X},{boardLocation.Y})";
        }
    }
}
