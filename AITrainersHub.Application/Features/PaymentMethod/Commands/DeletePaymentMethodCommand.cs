using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PaymentMethod.Commands
{
    /// <summary>
    /// Represents a command to delete a payment method.
    /// </summary>
    public class DeletePaymentMethodCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPaymentMethodsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePaymentMethodCommand()
        {
        }

        public DeletePaymentMethodCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadPaymentMethodByIdQuery)}_{id}");
        }
    }
}
