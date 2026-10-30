using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformCountry.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdatePlatformCountryCommand.
    /// </summary>
    public class UpdatePlatformCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdatePlatformCountryCommand, PlatformCountryDto>
    {
        public async Task<PlatformCountryDto> Handle(
            UpdatePlatformCountryCommand request,
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

            var country = await context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.CountryId, cancellationToken);

            platformCountry.Update(request.IsFullySupported, request.Notes);

            context.PlatformCountries.Update(platformCountry);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadCountriesByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return new PlatformCountryDto
            {
                CountryId = platformCountry.CountryId,
                NameAr = country?.NameAr ?? string.Empty,
                NameEn = country?.NameEn ?? string.Empty,
                IsoCode = country?.IsoCode ?? string.Empty,
                IsFullySupported = platformCountry.IsFullySupported,
                Notes = platformCountry.Notes
            };
        }
    }
}
