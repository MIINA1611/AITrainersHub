using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PaymentMethod.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPaymentMethodsQuery.
    /// </summary>
    public class ReadPaymentMethodsQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPaymentMethodsQuery, List<PaymentMethodDto>>
    {
        public async Task<List<PaymentMethodDto>> Handle(
            ReadPaymentMethodsQuery request,
            CancellationToken cancellationToken)
        {
            return await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(pm => pm.Name)
                .Select(pm => new PaymentMethodDto
                {
                    Id = pm.Id,
                    Name = pm.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
