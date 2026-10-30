using AITrainersHub.Application.Features.OnboardingStep.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.OnboardingStep.Validators
{
    /// <summary>
    /// Validates the UpdateOnboardingStepCommand.
    /// </summary>
    public class UpdateOnboardingStepCommandValidator : AbstractValidator<UpdateOnboardingStepCommand>
    {
        public UpdateOnboardingStepCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Step Id is required.");

            RuleFor(x => x.StepNumber)
                .GreaterThan(0).WithMessage("Step number must be greater than zero.");

            RuleFor(x => x.TitleAr)
                .NotEmpty().WithMessage("Arabic title is required.")
                .MaximumLength(150).WithMessage("Arabic title must not exceed 150 characters.");

            RuleFor(x => x.TitleEn)
                .NotEmpty().WithMessage("English title is required.")
                .MaximumLength(150).WithMessage("English title must not exceed 150 characters.");

            RuleFor(x => x.DescriptionAr)
                .NotEmpty().WithMessage("Arabic description is required.")
                .MaximumLength(1000).WithMessage("Arabic description must not exceed 1000 characters.");

            RuleFor(x => x.DescriptionEn)
                .NotEmpty().WithMessage("English description is required.")
                .MaximumLength(1000).WithMessage("English description must not exceed 1000 characters.");

            RuleFor(x => x.Tip)
                .MaximumLength(300).WithMessage("Tip must not exceed 300 characters.");

            RuleFor(x => x.ScreenshotUrl)
                .MaximumLength(300).WithMessage("Screenshot URL must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("Screenshot URL is invalid. It must start with http or https.")
                .When(x => !string.IsNullOrWhiteSpace(x.ScreenshotUrl));
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
