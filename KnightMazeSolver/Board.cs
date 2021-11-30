using FluentValidation;
using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.IO;

namespace KnightMazeSolver
{
    public class Board : IBoard
    {       
        internal IBoardLocation _startingLocation = null;
        internal IBoardLocation _endingLocation = null;
        internal SquareColor[,] _boardData = null;

        public byte Width { get; internal set; } = 0;
        public byte Height { get; internal set; } = 0;
        public byte MinimumBoardSize { get; } = 5;

        public IKnight Knight { get; } = null;
        
        public SquareColor this[byte x, byte y]
        {
            get 
            {
                if (!IsSquareInBounds(new BoardLocation(x, y)))
                {
                    throw new ArgumentOutOfRangeException($"Location ({x}, {y} Out of Range");
                }

                if (_boardData == null)
                {
                    throw new NullReferenceException($"Invalid board data.  Board data is uninitilized");
                }

                return _boardData[x - 1, y - 1];
            }
            set
            {
                if (!IsSquareInBounds(new BoardLocation(x, y)))
                {
                    throw new ArgumentOutOfRangeException($"Location ({x}, {y} Out of Range");
                }

                if (_boardData == null)
                {
                    throw new NullReferenceException($"Invalid board data.  Board data is uninitilized");
                }

                _boardData[x - 1, y - 1]  = value;
            }
        }

        public SquareColor this[IBoardLocation location]
        {
            get
            {
                return this[location.X, location.Y];
            }
            set
            {
                this[location.X, location.Y] = value;
            }
        }

        public IBoardLocation StartingLocation
        {
            get
            {
                return _startingLocation;
            }
            set
            {
                if (!IsSquareInBounds(value))
                {
                    throw new ArgumentOutOfRangeException($"Starting Location ({value.X},{value.Y}) Out of Range");
                }

                if (value.Equals(EndingLocation))
                {
                    throw new ArgumentException($"Starting Location ({value.X},{value.Y}) cannot equal ending location");
                }

                if (_boardData == null)
                {
                    throw new NullReferenceException($"Invalid board data.  Board data is uninitilized");
                }

                this[value] = GetSquareColor(value);                
                _startingLocation = value;
            }
        }

        public IBoardLocation EndingLocation
        {
            get
            {
                return _endingLocation;
            }
            set
            {
                if (!IsSquareInBounds(value))
                {
                    throw new ArgumentOutOfRangeException($"Ending Location ({value.X},{value.Y}) Out of Range");
                }

                if (value.Equals(StartingLocation))
                {
                    throw new ArgumentException($"Ending Location ({value.X},{value.Y}) cannot equal starting location");
                }

                if (_boardData == null)
                {
                    throw new NullReferenceException($"Invalid board data.  Board data is uninitilized");
                }

                this[value] = GetSquareColor(value); 
                _endingLocation = value;
            }
        }

        public Board(IKnight knight) => Knight = knight;

        public void Initialize(byte width, byte height)
        {
            if (width < MinimumBoardSize || height < MinimumBoardSize)
            {
                throw new ArgumentOutOfRangeException($"Invalid board size {width}x{height}.  Board must be at least {MinimumBoardSize} in each dimension.");
            }

            _boardData = new SquareColor[width, height];
            Width = width;
            Height = height;

            Knight.Initialize(this);
        }

        public void LoadStateData(string[] rows)
        {
            Width = 0;
            Height = 0;

            if (rows.Length > byte.MaxValue)
            {
                throw new InvalidDataException($"Invalid board data.  Row count exceeds {byte.MaxValue} square limit.");
            }

            if (rows.Length < MinimumBoardSize || rows[0].Length < MinimumBoardSize)
            {
                throw new InvalidDataException($"Invalid board data.  Board must be at least {MinimumBoardSize} in each dimension.");
            }

            if (rows[0].Length > byte.MaxValue)
            {
                throw new InvalidDataException($"Invalid board data.  Line length exceeds {byte.MaxValue} square limit.");
            }

            Width = (byte)rows[0].Length;
            Height = (byte)rows.Length;
            _boardData = new SquareColor[Width, Height];

            byte index = 1;
            foreach (string row in rows)
            {
                if (row.Length != Width)
                {
                    throw new InvalidDataException($"Invalid board data.  Inconsistent line length.");
                }

                ProcessRow(row, index++);
            }

            List<string> validationErrors;
            if (!ValidateBoard(out validationErrors))
            {
                throw new InvalidDataException(string.Join("\n", validationErrors));
            }

            Knight.Initialize(this);
        }

        private void ProcessRow(string row, byte index)
        {
            byte x = 0;
            foreach (char c in row)
            {
                x++;

                switch (c)
                {
                    case 'S':
                        this[x, index] = GetSquareColor(new BoardLocation(x, index));
                        if (_startingLocation != null)
                        {
                            throw new InvalidDataException($"Invalid board data.  Only one Starting postion may be specified");
                        }
                        _startingLocation = new BoardLocation(x, index);
                        break;

                    case 'E':
                        this[x, index] = GetSquareColor(new BoardLocation(x, index));
                        if (_endingLocation != null)
                        {
                            throw new InvalidDataException($"Invalid board data.  Only one Starting postion may be specified");
                        }
                        _endingLocation = new BoardLocation(x, index);
                        break;

                    case 'X':
                        this[x, index] = GetSquareColor(new BoardLocation(x, index));
                        break;

                    case '.':
                        this[x, index] = SquareColor.Void;
                        break;

                    default:
                        throw new InvalidDataException($"Invalid board data.  Invalid character '{c}'");
                }
            }
        }

        public bool IsSquareInBounds(IBoardLocation boardLocation)
        {
            return boardLocation != null && (boardLocation.X > 0 && boardLocation.X <= Width && boardLocation.Y > 0 && boardLocation.Y <= Height);
        }

        public bool IsValidTargetSquare(IBoardLocation boardLocation)
        {
            if (!IsSquareInBounds(boardLocation))
            {
                return false;
            }

            return this[boardLocation] != SquareColor.Void;
        }
        public bool ValidateBoard(out List<string> messages)
        {
            messages = new List<string>();

            BoardValidator validator = new BoardValidator();
            ValidationResult results = validator.Validate(this);

            if (results.IsValid)
            {
                return true;
            }

            foreach (ValidationFailure error in results.Errors)
            {
                messages.Add(error.ErrorMessage);
            }

            return false;
        }

        private SquareColor GetSquareColor(IBoardLocation boardLocation)
        {
            if (boardLocation.Y % 2 == 1)
            {
                return (boardLocation.X % 2 == 1) ? SquareColor.White : SquareColor.Black;
            }
            else
            {
                return (boardLocation.X % 2 == 0) ? SquareColor.White : SquareColor.Black;
            }
        }
    }
}