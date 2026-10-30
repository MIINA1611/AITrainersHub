using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Commands;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePlatformPaymentMethodCommand and removes the link.
    /// </summary>
    public class DeletePlatformPaymentMethodCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePlatformPaymentMethodCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePlatformPaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            var platformPaymentMethod = await context.PlatformPaymentMethods
                .FirstOrDefaultAsync(
                    ppm => ppm.PlatformId == request.PlatformId && ppm.PaymentMethodId == request.PaymentMethodId,
                    cancellationToken);

            if (platformPaymentMethod == null)
            {
                throw new ResultException("Platform payment method link not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            context.PlatformPaymentMethods.Remove(platformPaymentMethod);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPaymentMethodsByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return true;
        }
    }
}
