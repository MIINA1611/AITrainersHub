using AITrainerHub.Request_Models.Leader;
using AITrainersHub.Application.Features.Platform.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Backing model for the Leader opportunity create/edit form.
    /// </summary>
    public class LeaderOpportunityFormViewModel
    {
        public OpportunityFormRequest Form { get; set; } = new();
        public List<PlatformListResponse> Platforms { get; set; } = new();
        public bool IsEdit { get; set; }
        public string FormAction { get; set; } = "Create";
        public string FormTitle { get; set; } = "New Opportunity";
        public string FormSubtitle { get; set; } = string.Empty;
    }
}
