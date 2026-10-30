using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.VideoGuide.DTOs;

namespace AITrainersHub.Application.Features.Platform.DTOs
{
    /// <summary>
    /// Represents the full platform details, including all related collections.
    /// </summary>
    public class PlatformDetailsDto : PlatformDto
    {
        public List<PlatformCategoryDto> Categories { get; set; } = new();
        public List<PlatformPaymentMethodDto> PaymentMethods { get; set; } = new();
        public List<PlatformCountryDto> Countries { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
        public List<VideoGuideDto> VideoGuides { get; set; } = new();
        public List<OnboardingStepDto> OnboardingSteps { get; set; } = new();
    }
}
