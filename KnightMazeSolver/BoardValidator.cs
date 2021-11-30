using FluentValidation;

namespace KnightMazeSolver
{
    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(board => board.Width).GreaterThanOrEqualTo(board => board.MinimumBoardSize).WithMessage(board => $"Board is less than the minimum width of {board.MinimumBoardSize}");
            RuleFor(board => board.Height).GreaterThanOrEqualTo(board => board.MinimumBoardSize).WithMessage(board => $"Board is less than the minimum height of { board.MinimumBoardSize}");
            RuleFor(board => board._boardData).NotNull().WithMessage("Board data must be initialized");            
            RuleFor(board => board).Must(ValidateDataLength).WithMessage("Board data is incorrect size for specified width and height");
            RuleFor(board => board).Must(ValidateStartingLocation).WithMessage("Starting location is invalid");
            RuleFor(board => board).Must(ValidateEndingLocation).WithMessage("Ending location is invalid");
        }

        private bool ValidateDataLength(Board board)
        {
            return (board.Width * board.Height) == board._boardData.Length;
        }

        private bool ValidateStartingLocation(Board board)
        {
            return board.IsValidTargetSquare(board.StartingLocation);
        }

        private bool ValidateEndingLocation(Board board)
        {
            return board.IsValidTargetSquare(board.EndingLocation);
        }
    }
}
