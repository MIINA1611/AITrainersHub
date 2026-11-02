using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.DTOs
{
    /// <summary>
    /// Represents a platform row returned by the platforms list query.
    /// </summary>
    public class PlatformListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? ReferralLink { get; set; }
        public PlatformStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PlatformCategoryDto> Categories { get; set; } = new();
        public List<PlatformPaymentMethodDto> PaymentMethods { get; set; } = new();
        public List<PlatformCountryDto> Countries { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
