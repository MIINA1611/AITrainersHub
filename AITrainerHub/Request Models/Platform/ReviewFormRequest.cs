using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used to create a platform review from the Admin area.
    /// </summary>
    public class ReviewFormRequest
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Reviewer name is required.")]
        [StringLength(100, ErrorMessage = "Reviewer name must not exceed 100 characters.")]
        public string ReviewerName { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(1000, ErrorMessage = "Comment must not exceed 1000 characters.")]
        public string Comment { get; set; } = string.Empty;
    }
}
