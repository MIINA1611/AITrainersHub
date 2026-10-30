using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformCountryEntity = AITrainersHub.Domain.Models.PlatformCountry;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformCountry.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPlatformCountryCommand and creates the link.
    /// </summary>
    public class AddPlatformCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPlatformCountryCommand, PlatformCountryDto>
    {
        public async Task<PlatformCountryDto> Handle(
            AddPlatformCountryCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var country = await context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.CountryId, cancellationToken);

            if (country == null)
            {
                throw new ResultException($"Country with Id {request.CountryId} not found.", (int)HttpStatusCode.NotFound);
            }

            var platformCountry = new PlatformCountryEntity(
                request.PlatformId,
                request.CountryId,
                request.IsFullySupported,
                request.Notes);

            await context.PlatformCountries.AddAsync(platformCountry, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, platform);

            return new PlatformCountryDto
            {
                CountryId = country.Id,
                NameAr = country.NameAr,
                NameEn = country.NameEn,
                IsoCode = country.IsoCode,
                IsFullySupported = platformCountry.IsFullySupported,
                Notes = platformCountry.Notes
            };
        }

        private static void AddCacheKeys(AddPlatformCountryCommand request, PlatformEntity platform)
        {
            request.CacheKeySet.Add($"{nameof(ReadCountriesByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
        }
    }
}
