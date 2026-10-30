using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used to link a category to a platform.
    /// </summary>
    public class PlatformCategoryLinkRequest
    {
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public Guid CategoryId { get; set; }
    }

    /// <summary>
    /// Form model used to link or update a country for a platform.
    /// </summary>
    public class PlatformCountryLinkRequest
    {
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Please select a country.")]
        public Guid CountryId { get; set; }

        public bool IsFullySupported { get; set; } = true;

        [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters.")]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Form model used to link a payment method to a platform.
    /// </summary>
    public class PlatformPaymentMethodLinkRequest
    {
        public Guid PlatformId { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        public Guid PaymentMethodId { get; set; }
    }
}
