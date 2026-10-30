using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.OnboardingStep.Commands
{
    /// <summary>
    /// Represents a command to update an existing onboarding step.
    /// </summary>
    public class UpdateOnboardingStepCommand : IRequest<OnboardingStepDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Tip { get; set; }
        public string? ScreenshotUrl { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateOnboardingStepCommand()
        {
        }

        public UpdateOnboardingStepCommand(
            Guid id,
            int stepNumber,
            string titleAr,
            string titleEn,
            string descriptionAr,
            string descriptionEn,
            string? tip,
            string? screenshotUrl)
        {
            Id = id;
            StepNumber = stepNumber;
            TitleAr = titleAr;
            TitleEn = titleEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            Tip = tip;
            ScreenshotUrl = screenshotUrl;
            CacheKeySet.Add($"{nameof(ReadOnboardingStepByIdQuery)}_{id}");
        }
    }
}
