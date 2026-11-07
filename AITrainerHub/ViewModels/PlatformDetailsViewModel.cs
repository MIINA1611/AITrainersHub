using AITrainerHub.Request_Models.Public;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the public platform details page.
    /// </summary>
    public class PlatformDetailsViewModel
    {
        public PlatformDetailsDto Platform { get; set; } = null!;
        public List<WorkOpportunityDto> RelatedOpportunities { get; set; } = new();
        public PublicReviewRequest ReviewForm { get; set; } = new();
    }
}
