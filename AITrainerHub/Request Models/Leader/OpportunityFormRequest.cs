using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models.Leader
{
    /// <summary>
    /// Form model used by the Leader area to create or edit a work opportunity.
    /// Approval is intentionally not part of this model; only administrators
    /// can approve an opportunity.
    /// </summary>
    public class OpportunityFormRequest
    {
        public Guid Id { get; set; }

        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Opportunity title is required.")]
        [StringLength(150, ErrorMessage = "Opportunity title must not exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arabic description is required.")]
        [StringLength(2000, ErrorMessage = "Arabic description must not exceed 2000 characters.")]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English description is required.")]
        [StringLength(2000, ErrorMessage = "English description must not exceed 2000 characters.")]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "WhatsApp application link is required.")]
        [StringLength(300, ErrorMessage = "WhatsApp link must not exceed 300 characters.")]
        [Url(ErrorMessage = "WhatsApp link must be a valid absolute URL.")]
        public string WhatsAppLink { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
