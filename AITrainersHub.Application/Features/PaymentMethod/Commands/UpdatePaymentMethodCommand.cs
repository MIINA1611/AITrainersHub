using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PaymentMethod.Commands
{
    /// <summary>
    /// Represents a command to update an existing payment method.
    /// </summary>
    public class UpdatePaymentMethodCommand : IRequest<PaymentMethodDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPaymentMethodsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdatePaymentMethodCommand()
        {
        }

        public UpdatePaymentMethodCommand(Guid id, string? name)
        {
            Id = id;
            Name = name;
            CacheKeySet.Add($"{nameof(ReadPaymentMethodByIdQuery)}_{id}");
        }
    }
}
