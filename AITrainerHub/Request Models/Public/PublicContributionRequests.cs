using System.ComponentModel.DataAnnotations;

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
    /// Platform suggestion submitted anonymously by a community member.
    /// </summary>
    public class PlatformSuggestionRequest
    {
        [Required(ErrorMessage = "Your name is required.")]
        [StringLength(100, ErrorMessage = "Your name must not exceed 100 characters.")]
        public string SubmitterName { get; set; } = string.Empty;

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

        public Guid? CategoryId { get; set; }
    }
}
