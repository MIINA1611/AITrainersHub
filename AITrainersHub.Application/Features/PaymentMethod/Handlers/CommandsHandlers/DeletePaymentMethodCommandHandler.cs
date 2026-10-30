using System.Net;
using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PaymentMethod.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePaymentMethodCommand.
    /// </summary>
    public class DeletePaymentMethodCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePaymentMethodCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            var paymentMethod = await context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.Id == request.Id, cancellationToken);

            if (paymentMethod == null)
            {
                throw new ResultException($"Payment method with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.PaymentMethods.Remove(paymentMethod);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPaymentMethodByIdQuery)}_{paymentMethod.Id}");

            return true;
        }
    }
}
