using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used by the Reference Data payment method create/edit forms.
    /// </summary>
    public class PaymentMethodFormRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Payment method name is required.")]
        [StringLength(100, ErrorMessage = "Payment method name must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
