using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Validators
{
    /// <summary>
    /// Validates the UpdatePlatformSuggestionCommand.
    /// </summary>
    public class UpdatePlatformSuggestionCommandValidator : AbstractValidator<UpdatePlatformSuggestionCommand>
    {
        public UpdatePlatformSuggestionCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Suggestion Id is required.");

            RuleFor(x => x.PlatformName)
                .NotEmpty().WithMessage("Suggested platform name cannot be empty.")
                .MaximumLength(150).WithMessage("Suggested platform name must not exceed 150 characters.")
                .When(x => x.PlatformName is not null);

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Platform URL cannot be empty.")
                .MaximumLength(300).WithMessage("Platform URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Platform URL is invalid.")
                .When(x => x.Url is not null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Suggestion description cannot be empty.")
                .MaximumLength(1000).WithMessage("Suggestion description must not exceed 1000 characters.")
                .When(x => x.Description is not null);

            RuleFor(x => x.EmailOfUser)
                .MaximumLength(150).WithMessage("Email address must not exceed 150 characters.")
                .When(x => x.EmailOfUser is not null);

            RuleFor(x => x.EmailOfUser)
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
