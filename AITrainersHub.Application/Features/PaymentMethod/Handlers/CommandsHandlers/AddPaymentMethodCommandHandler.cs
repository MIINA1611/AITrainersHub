using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PaymentMethodEntity = AITrainersHub.Domain.Models.PaymentMethod;

namespace AITrainersHub.Application.Features.PaymentMethod.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPaymentMethodCommand and persists a new payment method.
    /// </summary>
    public class AddPaymentMethodCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPaymentMethodCommand, PaymentMethodDto>
    {
        public async Task<PaymentMethodDto> Handle(
            AddPaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            var paymentMethod = new PaymentMethodEntity(request.Name);

            await context.PaymentMethods.AddAsync(paymentMethod, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPaymentMethodByIdQuery)}_{paymentMethod.Id}");

            return Map(paymentMethod);
        }

        private static PaymentMethodDto Map(PaymentMethodEntity paymentMethod) => new()
        {
            Id = paymentMethod.Id,
            Name = paymentMethod.Name
        };
    }
}
