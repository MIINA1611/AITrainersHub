using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Country.Queries
{
    /// <summary>
    /// Represents a query to read a country by its unique identifier.
    /// </summary>
    public class ReadCountryByIdQuery : IRequest<CountryDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadCountryByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadCountryByIdQuery()
        {
        }

        public ReadCountryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
