namespace AITrainersHub.Application.Features.PaymentMethod.DTOs
{
    /// <summary>
    /// Represents a payment method returned by the payment method queries and commands.
    /// </summary>
    public class PaymentMethodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
