using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Domain.Enums;
using FluentValidation;

namespace AITrainersHub.Application.Features.VideoGuide.Validators
{
    /// <summary>
    /// Validates the UpdateVideoGuideCommand.
    /// </summary>
    public class UpdateVideoGuideCommandValidator : AbstractValidator<UpdateVideoGuideCommand>
    {
        public UpdateVideoGuideCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Guide Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Video title cannot be empty.")
                .MaximumLength(200).WithMessage("Video title must not exceed 200 characters.")
                .When(x => x.Title is not null);

            RuleFor(x => x.Language)
                .Must(language => !language.HasValue || Enum.IsDefined(typeof(GuideLanguage), language.Value))
                .WithMessage("Guide language is invalid.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Video URL cannot be empty.")
                .MaximumLength(500).WithMessage("Video URL must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("Video URL is invalid. It must start with http or https.")
                .When(x => x.Url is not null);

            RuleFor(x => x.SourceType)
                .NotEmpty().WithMessage("Source type cannot be empty.")
                .MaximumLength(50).WithMessage("Source type must not exceed 50 characters.")
                .When(x => x.SourceType is not null);

            RuleFor(x => x.AuthorOrChannel)
                .MaximumLength(150).WithMessage("Author or channel name must not exceed 150 characters.")
                .When(x => x.AuthorOrChannel is not null);
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
