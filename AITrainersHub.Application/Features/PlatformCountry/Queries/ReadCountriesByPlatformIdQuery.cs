using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCountry.Queries
{
    /// <summary>
    /// Represents a query to read all countries linked to a platform.
    /// </summary>
    public class ReadCountriesByPlatformIdQuery : IRequest<List<PlatformCountryDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }

        public string CacheKey => $"{nameof(ReadCountriesByPlatformIdQuery)}_{PlatformId}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadCountriesByPlatformIdQuery()
        {
        }

        public ReadCountriesByPlatformIdQuery(Guid platformId)
        {
            PlatformId = platformId;
        }
    }
}
