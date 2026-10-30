namespace AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs
{
    /// <summary>
    /// Represents a payment method linked to a platform.
    /// </summary>
    public class PlatformPaymentMethodDto
    {
        public Guid PaymentMethodId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
