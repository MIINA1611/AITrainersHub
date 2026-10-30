using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.VideoGuide.Validators
{
    /// <summary>
    /// Validates the AddVideoGuideCommand.
    /// </summary>
    public class AddVideoGuideCommandValidator : AbstractValidator<AddVideoGuideCommand>
    {
        public AddVideoGuideCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.")
                .MustAsync(async (platformId, cancellationToken) =>
                    await context.Platforms.AnyAsync(p => p.Id == platformId, cancellationToken))
                .WithMessage("Platform does not exist.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Video title is required.")
                .MaximumLength(200).WithMessage("Video title must not exceed 200 characters.");

            RuleFor(x => x.Language)
                .Must(language => Enum.IsDefined(typeof(GuideLanguage), language))
                .WithMessage("Guide language is invalid.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Video URL is required.")
                .MaximumLength(500).WithMessage("Video URL must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("Video URL is invalid. It must start with http or https.");

            RuleFor(x => x.SourceType)
                .NotEmpty().WithMessage("Source type is required.")
                .MaximumLength(50).WithMessage("Source type must not exceed 50 characters.");

            RuleFor(x => x.AuthorOrChannel)
                .MaximumLength(150).WithMessage("Author or channel name must not exceed 150 characters.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
