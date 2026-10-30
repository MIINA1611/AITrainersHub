using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Commands
{
    /// <summary>
    /// Represents a command to link a payment method to a platform.
    /// </summary>
    public class AddPlatformPaymentMethodCommand : IRequest<PlatformPaymentMethodDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPlatformPaymentMethodCommand()
        {
        }

        public AddPlatformPaymentMethodCommand(Guid platformId, Guid paymentMethodId)
        {
            PlatformId = platformId;
            PaymentMethodId = paymentMethodId;
        }
    }
}
