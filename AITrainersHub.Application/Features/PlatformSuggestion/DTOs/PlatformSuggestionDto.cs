namespace AITrainersHub.Application.Features.PlatformSuggestion.DTOs
{
    /// <summary>
    /// Represents a platform suggestion returned by the suggestion queries and commands.
    /// </summary>
    public class PlatformSuggestionDto
    {
        public Guid Id { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? EmailOfUser { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
