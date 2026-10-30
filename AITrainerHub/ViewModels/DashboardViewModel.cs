namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Aggregated counters displayed on the Admin overview page.
    /// </summary>
    public class DashboardViewModel
    {
        public int PendingSuggestions { get; set; }
        public int PendingVideoGuides { get; set; }
        public int PendingReviews { get; set; }
        public int TotalLeaders { get; set; }
        public int TotalLivePlatforms { get; set; }
        public int PendingOpportunities { get; set; }

        public int TotalPending =>
            PendingSuggestions + PendingVideoGuides + PendingReviews + PendingOpportunities;
    }
}
