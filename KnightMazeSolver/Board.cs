using FluentValidation;
using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.IO;

namespace KnightMazeSolver
{    
    /// <summary>
    /// Represets a game board and contains data for all valid locations
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
        /// HACK HACK
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
        SquareColor this[byte x, byte y] { get;set; }

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
        /// <param name="width">The requested width of the board. The minimum value is 5.</param>
        /// <param name="height">The requested height of the board. The minimum value is 5.</param>
        void Initialize(byte width, byte height);

        /// <summary>
        /// Initializes the board with an array of strings representing board rows.
        /// There must be at least 5 rows, at least 5 characters wide and all must be the same length.
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

        bool ValidateBoard(out List<string> messages);
    }

    public class Board : IBoard
    {
        internal const byte _minBoardSize = 5;

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
            if (width < _minBoardSize || height < _minBoardSize)
            {
                throw new ArgumentOutOfRangeException($"Invalid board size {width}x{height}.  Board must be at least {_minBoardSize} in each dimension.");
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

            if (rows.Length < _minBoardSize || rows[0].Length < _minBoardSize)
            {
                throw new InvalidDataException($"Invalid board data.  Board must be at least {_minBoardSize} in each dimension.");
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

            if (StartingLocation == null)
            {
                throw new InvalidDataException($"Invalid board data.  Starting postion must be specified");
            }

            if (EndingLocation == null)
            {
                throw new InvalidDataException($"Invalid board data.  Ending postion must be specified");
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