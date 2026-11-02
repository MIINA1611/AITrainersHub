namespace AITrainersHub.Application.Features.WorkOpportunity.DTOs
{
    /// <summary>
    /// Represents a work opportunity returned by the work opportunity queries and commands.
    /// </summary>
    public class WorkOpportunityDto
    {
        public Guid Id { get; set; }
        public Guid LeaderId { get; set; }
        public string LeaderDisplayName { get; set; } = string.Empty;
        public Guid PlatformId { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string PlatformSlug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WhatsAppLink { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
