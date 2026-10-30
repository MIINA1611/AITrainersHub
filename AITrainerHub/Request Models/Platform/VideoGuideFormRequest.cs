using System.ComponentModel.DataAnnotations;
using AITrainersHub.Domain.Enums;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used to create or edit a platform video guide.
    /// </summary>
    public class VideoGuideFormRequest
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Video title is required.")]
        [StringLength(200, ErrorMessage = "Video title must not exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Guide language is required.")]
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
}
