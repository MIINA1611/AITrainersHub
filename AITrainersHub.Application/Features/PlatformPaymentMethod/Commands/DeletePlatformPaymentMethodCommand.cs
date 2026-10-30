using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Commands
{
    /// <summary>
    /// Represents a command to unlink a payment method from a platform.
    /// </summary>
    public class DeletePlatformPaymentMethodCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePlatformPaymentMethodCommand()
        {
        }

        public DeletePlatformPaymentMethodCommand(Guid platformId, Guid paymentMethodId)
        {
            PlatformId = platformId;
            PaymentMethodId = paymentMethodId;
        }
    }
}
