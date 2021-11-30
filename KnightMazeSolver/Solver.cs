using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnightMazeSolver
{
    public interface ISolver
    {
        List<List<IMove>> Solve(string filename, SolveType solveType);
        List<List<IMove>> Solve(IBoard board, SolveType solveType);
        List<List<IMove>> Solve(string[] rows, SolveType solveType);
    }

    public class Solver : ISolver
    {
        internal List<IMove> _currentMoves = new List<IMove>();
        internal List<List<IMove>> _solutions = new List<List<IMove>>();

        private int _movesEvaluated = 0;
        private int _longestChainEvaluated = 0;
        private int _deadEndNodes = 0;

        public List<List<IMove>> Solve(string filename, SolveType solveType)
        {
            string[] rows = null;

            try
            {
                if (!File.Exists(filename))
                {
                    throw new Exception($"File {filename} does not exist.");
                }

                rows = File.ReadAllLines(filename);
            }
            catch (Exception e)
            {
                throw new Exception($"Error reading from {filename}.", e);
            }

            return Solve(rows, solveType);
        }

        public List<List<IMove>> Solve(IBoard board, SolveType solveType)
        {
            if (board.Width == 0 || board.Height == 0)
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
                Move(board, boardLocation, solveType);
            }

            return _solutions.OrderBy(m => m.Count).ToList();
        }

        public List<List<IMove>> Solve(string[] rows, SolveType solveType)
        {
            IKnight knight = new Knight();
            IBoard board = new Board(knight);

            board.LoadStateData(rows);

            return Solve(board, solveType);            
        }

        private bool Move(IBoard board, IBoardLocation newBoardLocation, SolveType solveType)
        {
            _currentMoves.Add(board.Knight.Move(newBoardLocation));
            _movesEvaluated++;

            if (_currentMoves.Count > _longestChainEvaluated)
            {
                _longestChainEvaluated = _currentMoves.Count;
            }

            bool lengthExceedsBest = _currentMoves.Count > (_solutions.Count > 0 ? _solutions[0].Count : int.MaxValue);

            if (solveType == SolveType.Shortest && lengthExceedsBest)
            {
                BackUpOneMove(board);
                return false;
            }

            if (newBoardLocation.Equals(board.EndingLocation))
            {
                if (solveType == SolveType.Shortest)
                {
                    bool bestSolution = _currentMoves.Count < (_solutions.Count > 0 ? _solutions[0].Count : int.MaxValue);

                    if (bestSolution)
                    {
                        _solutions.Clear();
                        _solutions.Add(_currentMoves.ToList<IMove>());
                    }
                }
                else
                {
                    _solutions.Add(_currentMoves.ToList<IMove>());
                }

                BackUpOneMove(board);
                return true;
            }

            if (board.Knight.ValidMoves.Count == 1)
            {
                _deadEndNodes++;
            }
            else if (EvaluateNextMoves(board, solveType) && solveType == SolveType.First)
            {
                return true;
            }

            BackUpOneMove(board);

            return false;
        }

        private bool EvaluateNextMoves(IBoard board, SolveType solveType)
        {
            foreach (BoardLocation targetLocation in board.Knight.ValidMoves)
            {
                if (targetLocation.Equals(board.StartingLocation) ||
                    (_currentMoves.Where(l => l.EndingLocation.Equals(targetLocation)).Any()))
                {
                    continue;
                }

                if (Move(board, targetLocation, solveType) && solveType == SolveType.First)
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
