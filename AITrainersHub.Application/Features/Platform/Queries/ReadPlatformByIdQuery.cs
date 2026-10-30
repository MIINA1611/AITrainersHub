using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Platform.Queries
{
    /// <summary>
    /// Represents a query to read the full platform details by its unique identifier.
    /// </summary>
    public class ReadPlatformByIdQuery : IRequest<PlatformDetailsDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadPlatformByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPlatformByIdQuery()
        {
        }

        public ReadPlatformByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
