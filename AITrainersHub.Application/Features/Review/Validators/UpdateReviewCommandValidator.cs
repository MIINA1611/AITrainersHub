using AITrainersHub.Application.Features.Review.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Review.Validators
{
    /// <summary>
    /// Validates the UpdateReviewCommand.
    /// </summary>
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Review Id is required.");

            RuleFor(x => x.ReviewerName)
                .NotEmpty().WithMessage("Reviewer name cannot be empty.")
                .MaximumLength(100).WithMessage("Reviewer name must not exceed 100 characters.")
                .When(x => x.ReviewerName is not null);

            RuleFor(x => x.Rating)
                .Must(rating => !rating.HasValue || (rating.Value >= 1 && rating.Value <= 5))
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment cannot be empty.")
                .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.")
                .When(x => x.Comment is not null);
        }
    }
}
