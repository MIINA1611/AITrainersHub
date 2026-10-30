using AITrainersHub.Application.Features.Advertisement.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Advertisement.Validators
{
    /// <summary>
    /// Validates the AddAdvertisementCommand.
    /// </summary>
    public class AddAdvertisementCommandValidator : AbstractValidator<AddAdvertisementCommand>
    {
        public AddAdvertisementCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Advertisement title is required.")
                .MaximumLength(200).WithMessage("Advertisement title must not exceed 200 characters.");

            RuleFor(x => x.Subtitle)
                .MaximumLength(300).WithMessage("Subtitle must not exceed 300 characters.")
                .When(x => x.Subtitle is not null);

            RuleFor(x => x.CtaText)
                .NotEmpty().WithMessage("Call-to-action text is required.")
                .MaximumLength(50).WithMessage("Call-to-action text must not exceed 50 characters.");

            RuleFor(x => x.CtaUrl)
                .NotEmpty().WithMessage("Call-to-action URL is required.")
                .MaximumLength(300).WithMessage("Call-to-action URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Call-to-action URL is invalid.");

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Advertisement position is required.")
                .MaximumLength(50).WithMessage("Advertisement position must not exceed 50 characters.");

            RuleFor(x => x.EndDate)
                .Must((command, endDate) =>
                    !command.StartDate.HasValue ||
                    !endDate.HasValue ||
                    endDate.Value >= command.StartDate.Value)
                .WithMessage("End date must be on or after the start date.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
