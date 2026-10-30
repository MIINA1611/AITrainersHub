using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformCountry.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadCountriesByPlatformIdQuery.
    /// </summary>
    public class ReadCountriesByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadCountriesByPlatformIdQuery, List<PlatformCountryDto>>
    {
        public async Task<List<PlatformCountryDto>> Handle(
            ReadCountriesByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            return await context.PlatformCountries
                .AsNoTracking()
                .Where(pc => pc.PlatformId == request.PlatformId)
                .OrderBy(pc => pc.Country.NameEn)
                .Select(pc => new PlatformCountryDto
                {
                    CountryId = pc.CountryId,
                    NameAr = pc.Country.NameAr,
                    NameEn = pc.Country.NameEn,
                    IsoCode = pc.Country.IsoCode,
                    IsFullySupported = pc.IsFullySupported,
                    Notes = pc.Notes
                })
                .ToListAsync(cancellationToken);
        }
    }
}
