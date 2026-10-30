using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the moderation queue page and its four tabs.
    /// </summary>
    public class ModerationQueueViewModel
    {
        public List<WorkOpportunityDto> Opportunities { get; set; } = new();
        public List<PlatformSuggestionDto> Suggestions { get; set; } = new();
        public List<VideoGuideDto> VideoGuides { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();

        public Dictionary<Guid, string> LeaderNames { get; set; } = new();
        public Dictionary<Guid, string> PlatformNames { get; set; } = new();
        public Dictionary<Guid, string> CategoryNames { get; set; } = new();

        public string ActiveTab { get; set; } = "opportunities";
    }
}
