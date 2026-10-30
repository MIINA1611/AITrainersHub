using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Commands;
using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;
using PlatformPaymentMethodEntity = AITrainersHub.Domain.Models.PlatformPaymentMethod;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPlatformPaymentMethodCommand and creates the link.
    /// </summary>
    public class AddPlatformPaymentMethodCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPlatformPaymentMethodCommand, PlatformPaymentMethodDto>
    {
        public async Task<PlatformPaymentMethodDto> Handle(
            AddPlatformPaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var paymentMethod = await context.PaymentMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(pm => pm.Id == request.PaymentMethodId, cancellationToken);

            if (paymentMethod == null)
            {
                throw new ResultException($"Payment method with Id {request.PaymentMethodId} not found.", (int)HttpStatusCode.NotFound);
            }

            var platformPaymentMethod = new PlatformPaymentMethodEntity(request.PlatformId, request.PaymentMethodId);

            await context.PlatformPaymentMethods.AddAsync(platformPaymentMethod, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, platform);

            return new PlatformPaymentMethodDto
            {
                PaymentMethodId = paymentMethod.Id,
                Name = paymentMethod.Name
            };
        }

        private static void AddCacheKeys(AddPlatformPaymentMethodCommand request, PlatformEntity platform)
        {
            request.CacheKeySet.Add($"{nameof(ReadPaymentMethodsByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
        }
    }
}
