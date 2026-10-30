using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used to create or edit a platform onboarding step.
    /// </summary>
    public class OnboardingStepFormRequest
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }

        [Range(1, 999, ErrorMessage = "Step number must be between 1 and 999.")]
        public int StepNumber { get; set; } = 1;

        [Required(ErrorMessage = "Arabic title is required.")]
        [StringLength(150, ErrorMessage = "Arabic title must not exceed 150 characters.")]
        public string TitleAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English title is required.")]
        [StringLength(150, ErrorMessage = "English title must not exceed 150 characters.")]
        public string TitleEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arabic description is required.")]
        [StringLength(1000, ErrorMessage = "Arabic description must not exceed 1000 characters.")]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English description is required.")]
        [StringLength(1000, ErrorMessage = "English description must not exceed 1000 characters.")]
        public string DescriptionEn { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Tip must not exceed 300 characters.")]
        public string? Tip { get; set; }

        [StringLength(300, ErrorMessage = "Screenshot URL must not exceed 300 characters.")]
        [Url(ErrorMessage = "Screenshot URL must be a valid absolute URL.")]
        public string? ScreenshotUrl { get; set; }
    }
}
