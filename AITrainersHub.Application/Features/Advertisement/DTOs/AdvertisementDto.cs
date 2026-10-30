namespace AITrainersHub.Application.Features.Advertisement.DTOs
{
    /// <summary>
    /// Represents an advertisement returned by the advertisement queries and commands.
    /// </summary>
    public class AdvertisementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string CtaText { get; set; } = string.Empty;
        public string CtaUrl { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
