using System;
using System.Collections.Generic;

namespace KnightMazeSolver
{
    public class Knight : IKnight
    {
        internal IBoard _board;
        internal List<IBoardLocation> _validMoves = null;

        public IBoardLocation BoardLocation { get; internal set; }

        public List<IBoardLocation> ValidMoves
        {
            get
            {
                if (_validMoves == null)
                {
                    _validMoves = new List<IBoardLocation>();

                    AddMoveIfValid((byte)(BoardLocation.X - 2), (byte)(BoardLocation.Y - 1));
                    AddMoveIfValid((byte)(BoardLocation.X - 1), (byte)(BoardLocation.Y - 2));
                    AddMoveIfValid((byte)(BoardLocation.X + 1), (byte)(BoardLocation.Y - 2));
                    AddMoveIfValid((byte)(BoardLocation.X + 2), (byte)(BoardLocation.Y - 1));
                    AddMoveIfValid((byte)(BoardLocation.X + 2), (byte)(BoardLocation.Y + 1));
                    AddMoveIfValid((byte)(BoardLocation.X + 1), (byte)(BoardLocation.Y + 2));
                    AddMoveIfValid((byte)(BoardLocation.X - 1), (byte)(BoardLocation.Y + 2));
                    AddMoveIfValid((byte)(BoardLocation.X - 2), (byte)(BoardLocation.Y + 1));
                }

                return _validMoves;
            }
        }

        public void Initialize(IBoard board)
        {            
            if (board == null)
            {
                throw new NullReferenceException("Board must not be null");
            }

            _board = board;
            BoardLocation = _board.StartingLocation;
        }

        public IMove Move(IBoardLocation targetLocation)
        {
            if (!_board.IsValidTargetSquare(targetLocation))
            {
                throw new ArgumentOutOfRangeException($"Invalid Move Target From ({BoardLocation.X},{BoardLocation.Y}) To ({targetLocation.X},{targetLocation.Y})");
            }

            byte startX = BoardLocation.X;
            byte startY = BoardLocation.Y;
            byte targetX = targetLocation.X;
            byte targetY = targetLocation.Y;

            if    (!((targetX == startX - 2 && targetY == startY - 1) ||
                    (targetX == startX - 2 && targetY == startY + 1) ||
                    (targetX == startX - 1 && targetY == startY - 2) ||
                    (targetX == startX - 1 && targetY == startY + 2) ||
                    (targetX == startX + 1 && targetY == startY - 2) ||
                    (targetX == startX + 1 && targetY == startY + 2) ||
                    (targetX == startX + 2 && targetY == startY - 1) ||
                    (targetX == startX + 2 && targetY == startY + 1)))
            {
                throw new ArgumentOutOfRangeException($"Invalid Move From ({BoardLocation.X},{BoardLocation.Y}) To ({targetLocation.X},{targetLocation.Y})");
            }            

            Move move = new Move(BoardLocation, targetLocation);

            BoardLocation = targetLocation;
            _validMoves = null;

            return move;
        }

        private void AddMoveIfValid(byte x, byte y)
        {
            BoardLocation moveLocation = new BoardLocation(x, y);

            if (_board.IsValidTargetSquare(moveLocation))
                _validMoves.Add(moveLocation);
        }
    }
}
