using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the public opportunities board.
    /// </summary>
    public class OpportunitiesIndexViewModel
    {
        public List<WorkOpportunityDto> Opportunities { get; set; } = new();
        public List<PlatformListResponse> Platforms { get; set; } = new();
        public string? Platform { get; set; }
        public string? Search { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; } = 1;
    }
}
