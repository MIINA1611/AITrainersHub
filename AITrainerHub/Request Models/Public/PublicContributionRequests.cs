using System.ComponentModel.DataAnnotations;
using AITrainersHub.Domain.Enums;

namespace AITrainerHub.Request_Models.Public
{
    /// <summary>
    /// Review submitted from the public platform details page.
    /// </summary>
    public class PublicReviewRequest
    {
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Reviewer name is required.")]
        [StringLength(100, ErrorMessage = "Reviewer name must not exceed 100 characters.")]
        public string ReviewerName { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "Review comment is required.")]
        [StringLength(1000, ErrorMessage = "Review comment must not exceed 1000 characters.")]
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// Video guide submitted from the public platform details page.
    /// </summary>
    public class PublicVideoGuideRequest
    {
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Video title is required.")]
        [StringLength(200, ErrorMessage = "Video title must not exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        public GuideLanguage Language { get; set; } = GuideLanguage.Arabic;

        [Required(ErrorMessage = "Video URL is required.")]
        [StringLength(500, ErrorMessage = "Video URL must not exceed 500 characters.")]
        [Url(ErrorMessage = "Video URL must be a valid absolute URL.")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Source type is required.")]
        [StringLength(50, ErrorMessage = "Source type must not exceed 50 characters.")]
        public string SourceType { get; set; } = "YouTube";

        [StringLength(150, ErrorMessage = "Author or channel name must not exceed 150 characters.")]
        public string? AuthorOrChannel { get; set; }

        public DateTime? PublishedAt { get; set; }
    }

    /// <summary>
    /// Platform suggestion submitted by an authenticated community member.
    /// </summary>
    public class PlatformSuggestionRequest
    {
        [Required(ErrorMessage = "Suggested platform name is required.")]
        [StringLength(150, ErrorMessage = "Suggested platform name must not exceed 150 characters.")]
        public string PlatformName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Platform URL is required.")]
        [StringLength(300, ErrorMessage = "Platform URL must not exceed 300 characters.")]
        [Url(ErrorMessage = "Platform URL must be a valid absolute URL.")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please tell us why this platform is useful.")]
        [StringLength(1000, ErrorMessage = "Suggestion description must not exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Email address must not exceed 150 characters.")]
        [EmailAddress(ErrorMessage = "Email address is invalid.")]
        public string? EmailOfUser { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
