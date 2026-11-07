using AITrainerHub.Request_Models.Public;
using FluentValidation;

namespace AITrainerHub.Validators
{
    /// <summary>
    /// Validates a public review submission.
    /// </summary>
    public class PublicReviewRequestValidator : AbstractValidator<PublicReviewRequest>
    {
        public PublicReviewRequestValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform is required.");

            RuleFor(x => x.ReviewerName)
                .NotEmpty().WithMessage("Reviewer name is required.")
                .MaximumLength(100).WithMessage("Reviewer name must not exceed 100 characters.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Review comment is required.")
                .MaximumLength(1000).WithMessage("Review comment must not exceed 1000 characters.");
        }
    }

    /// <summary>
    /// Validates a platform suggestion submission.
    /// </summary>
    public class PlatformSuggestionRequestValidator : AbstractValidator<PlatformSuggestionRequest>
    {
        public PlatformSuggestionRequestValidator()
        {
            RuleFor(x => x.SubmitterName)
                .NotEmpty().WithMessage("Your name is required.")
                .MaximumLength(100).WithMessage("Your name must not exceed 100 characters.");

            RuleFor(x => x.PlatformName)
                .NotEmpty().WithMessage("Suggested platform name is required.")
                .MaximumLength(150).WithMessage("Suggested platform name must not exceed 150 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Platform URL is required.")
                .MaximumLength(300).WithMessage("Platform URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Platform URL must be a valid http(s) URL.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Please tell us why this platform is useful.")
                .MaximumLength(1000).WithMessage("Suggestion description must not exceed 1000 characters.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
