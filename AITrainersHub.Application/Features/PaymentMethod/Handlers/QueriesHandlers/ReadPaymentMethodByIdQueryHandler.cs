using System.Net;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PaymentMethod.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPaymentMethodByIdQuery.
    /// </summary>
    public class ReadPaymentMethodByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPaymentMethodByIdQuery, PaymentMethodDto>
    {
        public async Task<PaymentMethodDto> Handle(
            ReadPaymentMethodByIdQuery request,
            CancellationToken cancellationToken)
        {
            var paymentMethod = await context.PaymentMethods
                .AsNoTracking()
                .Where(pm => pm.Id == request.Id)
                .Select(pm => new PaymentMethodDto
                {
                    Id = pm.Id,
                    Name = pm.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (paymentMethod == null)
            {
                throw new ResultException($"Payment method with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return paymentMethod;
        }
    }
}
