using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.Validators
{
    /// <summary>
    /// Validates the UpdatePlatformCommand before the platform is updated.
    /// </summary>
    public class UpdatePlatformCommandValidator : AbstractValidator<UpdatePlatformCommand>
    {
        public UpdatePlatformCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Platform Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Platform name cannot be empty.")
                .MaximumLength(150).WithMessage("Platform name must not exceed 150 characters.")
                .MustAsync(async (command, name, cancellationToken) =>
                {
                    var normalizedName = (name ?? string.Empty).Trim();
                    return !await context.Platforms.AnyAsync(
                        p => p.Id != command.Id && p.Name == normalizedName,
                        cancellationToken);
                })
                .WithMessage("Another platform with the same name already exists.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug cannot be empty.")
                .MustAsync(async (command, slug, cancellationToken) =>
                {
                    var normalizedSlug = (slug ?? string.Empty).Trim();
                    return !await context.Platforms.AnyAsync(
                        p => p.Id != command.Id && p.Slug == normalizedSlug,
                        cancellationToken);
                })
                .WithMessage("Another platform with the same slug already exists.")
                .When(x => x.Slug is not null);

            RuleFor(x => x.DescriptionAr)
                .NotEmpty().WithMessage("Arabic description cannot be empty.")
                .MaximumLength(2000).WithMessage("Arabic description must not exceed 2000 characters.")
                .When(x => x.DescriptionAr is not null);

            RuleFor(x => x.DescriptionEn)
                .NotEmpty().WithMessage("English description cannot be empty.")
                .MaximumLength(2000).WithMessage("English description must not exceed 2000 characters.")
                .When(x => x.DescriptionEn is not null);

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty().WithMessage("Website URL cannot be empty.")
                .MaximumLength(300).WithMessage("Website URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Website URL is invalid. It must start with http or https.")
                .When(x => x.WebsiteUrl is not null);

            RuleFor(x => x.ReferralLink)
                .MaximumLength(300).WithMessage("Referral link must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Referral link is invalid. It must start with http or https.")
                .When(x => !string.IsNullOrWhiteSpace(x.ReferralLink));

            RuleFor(x => x.Status)
                .Must(status => !status.HasValue || Enum.IsDefined(typeof(PlatformStatus), status.Value))
                .WithMessage("Platform status is invalid.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
