using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnightMazeSolver
{
    public interface ISolver
    {
        List<List<IMove>> Solve(string filename, bool findAll = true);
        List<List<IMove>> Solve(IBoard board, bool findAll = true);
        List<List<IMove>> Solve(string[] rows, bool findAll = true);
    }

    public class Solver : ISolver
    {
        internal List<IMove> _currentMoves = new List<IMove>();
        internal List<List<IMove>> _solutions = new List<List<IMove>>();

        public List<List<IMove>> Solve(string filename, bool findAll = true)
        {
            if (!File.Exists(filename))
            {
                throw new Exception($"File {filename} does not exist.");
            }

            string[] rows = null;

            try
            {
                rows = File.ReadAllLines(filename);
            }
            catch (Exception e)
            {
                throw new Exception($"Error reading from {filename}.", e);
            }

            return Solve(rows, findAll);
        }

        public List<List<IMove>> Solve(IBoard board, bool findAll = true)
        {
            if (board.Width == 0  || board.Height == 0)
            {
                throw new Exception("Board must be intializzed.");
            }

            if (board.StartingLocation == null)
            {
                throw new InvalidDataException("Invalid board data.  No starting location.");
            }

            if (board.StartingLocation == null)
            {
                throw new InvalidDataException("Invalid board data.  No ending location.");
            }

            if (!board.Knight.BoardLocation.Equals(board.StartingLocation))
            {
                throw new InvalidDataException("Invalid board data.  Invalid knight location.");
            }

            if (board[board.StartingLocation] == SquareColor.Void)
            {
                throw new InvalidDataException("Invalid board data.  Starting location is invalid.");
            }

            if (board[board.EndingLocation] == SquareColor.Void)
            {
                throw new InvalidDataException("Invalid board data.  Ending location is invalid.");
            }

            foreach (BoardLocation boardLocation in board.Knight.ValidMoves)
            {
                Move(board, boardLocation, findAll);
            }

            return _solutions.OrderBy(m => m.Count).ToList();
        }

        public List<List<IMove>> Solve(string[] rows, bool findAll = true)
        {
            IKnight knight = new Knight();
            IBoard board = new Board(knight);

            board.LoadStateData(rows);

            return Solve(board, findAll);            
        }

        private bool Move(IBoard board, IBoardLocation newBoardLocation, bool findAll)
        {
            _currentMoves.Add(board.Knight.Move(newBoardLocation));

            if (newBoardLocation.Equals(board.EndingLocation))
            {                    
                _solutions.Add(_currentMoves.ToList<IMove>());
                BackUpOneMove(board);
                return true;
            }

            if (EvaluateNextMoves(board, findAll) && !findAll)
            {
                return true;
            }

            BackUpOneMove(board);

            return false;
        }

        private bool EvaluateNextMoves(IBoard board, bool findAll)
        {
            foreach (BoardLocation targetLocation in board.Knight.ValidMoves)
            {
                if (targetLocation.Equals(board.StartingLocation) ||
                    (_currentMoves.Where(l => l.EndingLocation.Equals(targetLocation)).Any()))
                {
                    continue;
                }

                if (Move(board, targetLocation, findAll) && !findAll)
                {
                    return true;
                }
            }

            return false;
        }

        private void BackUpOneMove(IBoard board)
        {
            board.Knight.Move(_currentMoves[_currentMoves.Count - 1].StartingLocation);
            _currentMoves.RemoveAt(_currentMoves.Count - 1);
        }
    }
}
