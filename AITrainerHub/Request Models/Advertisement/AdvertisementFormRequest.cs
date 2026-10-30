using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used by the Admin advertisement create/edit forms.
    /// </summary>
    public class AdvertisementFormRequest : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Advertisement title is required.")]
        [StringLength(200, ErrorMessage = "Advertisement title must not exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Subtitle must not exceed 300 characters.")]
        public string? Subtitle { get; set; }

        [Required(ErrorMessage = "Call-to-action text is required.")]
        [StringLength(50, ErrorMessage = "Call-to-action text must not exceed 50 characters.")]
        public string CtaText { get; set; } = "Book now";

        [Required(ErrorMessage = "Call-to-action URL is required.")]
        [StringLength(300, ErrorMessage = "Call-to-action URL must not exceed 300 characters.")]
        [Url(ErrorMessage = "Call-to-action URL must be a valid absolute URL.")]
        public string CtaUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Advertisement position is required.")]
        [StringLength(50, ErrorMessage = "Advertisement position must not exceed 50 characters.")]
        public string Position { get; set; } = "Hero";

        public bool IsActive { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate.Value < StartDate.Value)
            {
                yield return new ValidationResult(
                    "End date must be on or after the start date.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
