using AITrainersHub.Application.Features.Review.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Review.Validators
{
    /// <summary>
    /// Validates the DeleteReviewCommand.
    /// </summary>
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Review Id is required.");
        }
    }
}
