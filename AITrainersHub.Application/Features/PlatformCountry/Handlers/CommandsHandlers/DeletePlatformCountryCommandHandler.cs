using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Features.PlatformCountry.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformCountry.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePlatformCountryCommand and removes the link.
    /// </summary>
    public class DeletePlatformCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePlatformCountryCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePlatformCountryCommand request,
            CancellationToken cancellationToken)
        {
            var platformCountry = await context.PlatformCountries
                .FirstOrDefaultAsync(
                    pc => pc.PlatformId == request.PlatformId && pc.CountryId == request.CountryId,
                    cancellationToken);

            if (platformCountry == null)
            {
                throw new ResultException("Platform country link not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            context.PlatformCountries.Remove(platformCountry);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadCountriesByPlatformIdQuery)}_{request.PlatformId}");
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
