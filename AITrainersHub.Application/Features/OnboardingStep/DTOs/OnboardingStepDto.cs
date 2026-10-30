namespace AITrainersHub.Application.Features.OnboardingStep.DTOs
{
    /// <summary>
    /// Represents an onboarding step linked to a platform.
    /// </summary>
    public class OnboardingStepDto
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public int StepNumber { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Tip { get; set; }
        public string? ScreenshotUrl { get; set; }
    }
}
