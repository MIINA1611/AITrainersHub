using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Advertisement.Queries
{
    /// <summary>
    /// Represents a query to read an advertisement by its unique identifier.
    /// </summary>
    public class ReadAdvertisementByIdQuery : IRequest<AdvertisementDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadAdvertisementByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadAdvertisementByIdQuery()
        {
        }

        public ReadAdvertisementByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
