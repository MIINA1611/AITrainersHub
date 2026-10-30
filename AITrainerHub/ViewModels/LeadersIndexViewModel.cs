namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the leaders management page.
    /// </summary>
    public class LeadersIndexViewModel
    {
        public List<LeaderRowViewModel> Leaders { get; set; } = new();
    }

    /// <summary>
    /// One row in the leaders table.
    /// </summary>
    public class LeaderRowViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public int OpportunitiesCount { get; set; }
        public bool IsActive { get; set; }
    }
}
