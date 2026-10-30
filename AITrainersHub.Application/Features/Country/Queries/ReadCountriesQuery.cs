using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Country.Queries
{
    /// <summary>
    /// Represents a query to read all countries.
    /// </summary>
    public class ReadCountriesQuery : IRequest<List<CountryDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(ReadCountriesQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
