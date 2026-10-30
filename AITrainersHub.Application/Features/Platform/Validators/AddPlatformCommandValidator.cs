using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.Validators
{
    /// <summary>
    /// Validates the AddPlatformCommand before the platform is created.
    /// </summary>
    public class AddPlatformCommandValidator : AbstractValidator<AddPlatformCommand>
    {
        public AddPlatformCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Platform name is required.")
                .MaximumLength(150).WithMessage("Platform name must not exceed 150 characters.")
                .MustAsync(async (name, cancellationToken) =>
                {
                    var normalizedName = (name ?? string.Empty).Trim();
                    return !await context.Platforms.AnyAsync(p => p.Name == normalizedName, cancellationToken);
                })
                .WithMessage("A platform with the same name already exists.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MustAsync(async (slug, cancellationToken) =>
                {
                    var normalizedSlug = (slug ?? string.Empty).Trim();
                    return !await context.Platforms.AnyAsync(p => p.Slug == normalizedSlug, cancellationToken);
                })
                .WithMessage("A platform with the same slug already exists.");

            RuleFor(x => x.DescriptionAr)
                .NotEmpty().WithMessage("Arabic description is required.")
                .MaximumLength(2000).WithMessage("Arabic description must not exceed 2000 characters.");

            RuleFor(x => x.DescriptionEn)
                .NotEmpty().WithMessage("English description is required.")
                .MaximumLength(2000).WithMessage("English description must not exceed 2000 characters.");

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty().WithMessage("Website URL is required.")
                .MaximumLength(300).WithMessage("Website URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Website URL is invalid. It must start with http or https.");

            RuleFor(x => x.ReferralLink)
                .MaximumLength(300).WithMessage("Referral link must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Referral link is invalid. It must start with http or https.")
                .When(x => !string.IsNullOrWhiteSpace(x.ReferralLink));

            RuleFor(x => x.Status)
                .Must(status => Enum.IsDefined(typeof(PlatformStatus), status))
                .WithMessage("Platform status is invalid.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
