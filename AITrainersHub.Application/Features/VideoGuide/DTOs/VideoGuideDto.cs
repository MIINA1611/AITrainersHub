using AITrainersHub.Domain.Enums;

namespace AITrainersHub.Application.Features.VideoGuide.DTOs
{
    /// <summary>
    /// Represents a video guide linked to a platform.
    /// </summary>
    public class VideoGuideDto
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public string Title { get; set; } = string.Empty;
        public GuideLanguage Language { get; set; }
        public string Url { get; set; } = string.Empty;
        public string SourceType { get; set; } = string.Empty;
        public string? AuthorOrChannel { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
