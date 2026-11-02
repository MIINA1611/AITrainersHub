using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the public home page.
    /// </summary>
    public class PublicHomeViewModel
    {
        public List<PlatformListResponse> FeaturedPlatforms { get; set; } = new();
        public List<WorkOpportunityDto> LatestOpportunities { get; set; } = new();
        public List<CategoryListResponse> Categories { get; set; } = new();
        public Dictionary<Guid, int> CategoryCounts { get; set; } = new();
        public int TotalPlatforms { get; set; }
        public int TotalOpportunities { get; set; }
    }
}
