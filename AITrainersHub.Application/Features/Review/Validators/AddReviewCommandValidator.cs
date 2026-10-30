using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Review.Validators
{
    /// <summary>
    /// Validates the AddReviewCommand.
    /// </summary>
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.")
                .MustAsync(async (platformId, cancellationToken) =>
                    await context.Platforms.AnyAsync(p => p.Id == platformId, cancellationToken))
                .WithMessage("Platform does not exist.");

            RuleFor(x => x.ReviewerName)
                .NotEmpty().WithMessage("Reviewer name is required.")
                .MaximumLength(100).WithMessage("Reviewer name must not exceed 100 characters.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");
        }
    }
}
