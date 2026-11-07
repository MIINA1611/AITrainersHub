using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Validators
{
    /// <summary>
    /// Validates the AddPlatformSuggestionCommand.
    /// </summary>
    public class AddPlatformSuggestionCommandValidator : AbstractValidator<AddPlatformSuggestionCommand>
    {
        public AddPlatformSuggestionCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformName)
                .NotEmpty().WithMessage("Suggested platform name is required.")
                .MaximumLength(150).WithMessage("Suggested platform name must not exceed 150 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Platform URL is required.")
                .MaximumLength(300).WithMessage("Platform URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Platform URL is invalid.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Suggestion description is required.")
                .MaximumLength(1000).WithMessage("Suggestion description must not exceed 1000 characters.");

            RuleFor(x => x.SubmitterName)
                .NotEmpty().WithMessage("Your name is required.")
                .MaximumLength(100).WithMessage("Your name must not exceed 100 characters.");

            RuleFor(x => x.EmailOfUser)
                .MaximumLength(150).WithMessage("Email address must not exceed 150 characters.")
                .EmailAddress().WithMessage("Email address is invalid.")
                .When(x => !string.IsNullOrWhiteSpace(x.EmailOfUser));

            RuleFor(x => x.CategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                    !categoryId.HasValue ||
                    await context.Categories.AnyAsync(c => c.Id == categoryId.Value, cancellationToken))
                .WithMessage("Category does not exist.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
