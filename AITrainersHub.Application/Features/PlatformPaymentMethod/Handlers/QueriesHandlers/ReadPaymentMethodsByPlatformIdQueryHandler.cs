using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPaymentMethodsByPlatformIdQuery.
    /// </summary>
    public class ReadPaymentMethodsByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPaymentMethodsByPlatformIdQuery, List<PlatformPaymentMethodDto>>
    {
        public async Task<List<PlatformPaymentMethodDto>> Handle(
            ReadPaymentMethodsByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            return await context.PlatformPaymentMethods
                .AsNoTracking()
                .Where(ppm => ppm.PlatformId == request.PlatformId)
                .OrderBy(ppm => ppm.PaymentMethod.Name)
                .Select(ppm => new PlatformPaymentMethodDto
                {
                    PaymentMethodId = ppm.PaymentMethodId,
                    Name = ppm.PaymentMethod.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
