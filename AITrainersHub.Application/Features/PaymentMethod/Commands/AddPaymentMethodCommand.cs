using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PaymentMethod.Commands
{
    /// <summary>
    /// Represents a command to create a new payment method.
    /// </summary>
    public class AddPaymentMethodCommand : IRequest<PaymentMethodDto>, ICacheInvalidatorCommand
    {
        public string Name { get; set; } = string.Empty;

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPaymentMethodsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPaymentMethodCommand()
        {
        }

        public AddPaymentMethodCommand(string name)
        {
            Name = name;
        }
    }
}
