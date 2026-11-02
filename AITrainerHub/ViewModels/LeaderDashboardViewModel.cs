using AITrainersHub.Application.Features.WorkOpportunity.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the Leader dashboard.
    /// </summary>
    public class LeaderDashboardViewModel
    {
        public List<WorkOpportunityDto> Opportunities { get; set; } = new();
        public string DisplayName { get; set; } = string.Empty;

        public int Total => Opportunities.Count;
        public int Live => Opportunities.Count(opportunity => opportunity.IsApproved && opportunity.IsActive);
        public int Pending => Opportunities.Count(opportunity => !opportunity.IsApproved);
        public int Hidden => Opportunities.Count(opportunity => opportunity.IsApproved && !opportunity.IsActive);
    }
}
