using System.ComponentModel.DataAnnotations;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used by the Admin platform Create/Edit views.
    /// </summary>
    public class PlatformFormRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Platform name is required.")]
        [StringLength(150, ErrorMessage = "Platform name must not exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required.")]
        [StringLength(150, ErrorMessage = "Slug must not exceed 150 characters.")]
        [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug may contain lowercase letters, numbers, and hyphens only.")]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arabic description is required.")]
        [StringLength(2000, ErrorMessage = "Arabic description must not exceed 2000 characters.")]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English description is required.")]
        [StringLength(2000, ErrorMessage = "English description must not exceed 2000 characters.")]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Website URL is required.")]
        [StringLength(300, ErrorMessage = "Website URL must not exceed 300 characters.")]
        [Url(ErrorMessage = "Website URL must be a valid absolute URL.")]
        public string WebsiteUrl { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Referral link must not exceed 300 characters.")]
        [Url(ErrorMessage = "Referral link must be a valid absolute URL.")]
        public string? ReferralLink { get; set; }

        [Required(ErrorMessage = "Platform status is required.")]
        public PlatformStatus Status { get; set; } = PlatformStatus.Open;
    }
}
