using System.Collections.Generic;

namespace KnightMazeSolver
{
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
