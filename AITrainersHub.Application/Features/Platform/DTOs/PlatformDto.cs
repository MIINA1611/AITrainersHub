using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.DTOs
{
    /// <summary>
    /// Represents a platform's basic data returned by the add and update commands.
    /// </summary>
    public class PlatformDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? ReferralLink { get; set; }
        public PlatformStatus Status { get; set; } = PlatformStatus.Open;
        public DateTime CreatedAt { get; set; }
    }
}
