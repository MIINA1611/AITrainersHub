using System.Net;
using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentMethodEntity = AITrainersHub.Domain.Models.PaymentMethod;

namespace AITrainersHub.Application.Features.PaymentMethod.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdatePaymentMethodCommand and applies the supplied values.
    /// </summary>
    public class UpdatePaymentMethodCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethodDto>
    {
        public async Task<PaymentMethodDto> Handle(
            UpdatePaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            var paymentMethod = await context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.Id == request.Id, cancellationToken);

            if (paymentMethod == null)
            {
                throw new ResultException($"Payment method with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            paymentMethod.Update(request.Name);

            context.PaymentMethods.Update(paymentMethod);
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
