using AITrainerHub.Request_Models.Public;
using AITrainersHub.Domain.Enums;
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
    /// Validates a public video guide submission.
    /// </summary>
    public class PublicVideoGuideRequestValidator : AbstractValidator<PublicVideoGuideRequest>
    {
        public PublicVideoGuideRequestValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Video title is required.")
                .MaximumLength(200).WithMessage("Video title must not exceed 200 characters.");

            RuleFor(x => x.Language)
                .IsInEnum().WithMessage("Please choose a valid guide language.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Video URL is required.")
                .MaximumLength(500).WithMessage("Video URL must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("Video URL must be a valid http(s) URL.");

            RuleFor(x => x.SourceType)
                .NotEmpty().WithMessage("Source type is required.")
                .MaximumLength(50).WithMessage("Source type must not exceed 50 characters.");

            RuleFor(x => x.AuthorOrChannel)
                .MaximumLength(150).WithMessage("Author or channel name must not exceed 150 characters.");

            RuleFor(x => x.PublishedAt)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Published date cannot be in the future.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }

    /// <summary>
    /// Validates a platform suggestion submission.
    /// </summary>
    public class PlatformSuggestionRequestValidator : AbstractValidator<PlatformSuggestionRequest>
    {
        public PlatformSuggestionRequestValidator()
        {
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

            RuleFor(x => x.EmailOfUser)
                .MaximumLength(150).WithMessage("Email address must not exceed 150 characters.")
                .EmailAddress().WithMessage("Email address is invalid.")
                .When(x => !string.IsNullOrWhiteSpace(x.EmailOfUser));
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
