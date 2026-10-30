using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.OnboardingStep.Commands
{
    /// <summary>
    /// Represents a command to add a new onboarding step to a platform.
    /// </summary>
    public class AddOnboardingStepCommand : IRequest<OnboardingStepDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public int StepNumber { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Tip { get; set; }
        public string? ScreenshotUrl { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddOnboardingStepCommand()
        {
        }

        public AddOnboardingStepCommand(
            Guid platformId,
            int stepNumber,
            string titleAr,
            string titleEn,
            string descriptionAr,
            string descriptionEn,
            string? tip,
            string? screenshotUrl)
        {
            PlatformId = platformId;
            StepNumber = stepNumber;
            TitleAr = titleAr;
            TitleEn = titleEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            Tip = tip;
            ScreenshotUrl = screenshotUrl;
        }
    }
}
