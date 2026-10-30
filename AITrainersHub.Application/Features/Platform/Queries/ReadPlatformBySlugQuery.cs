using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Platform.Queries
{
    /// <summary>
    /// Represents a query to read the full platform details by its slug.
    /// </summary>
    public class ReadPlatformBySlugQuery : IRequest<PlatformDetailsDto>, ICacheableQuery
    {
        public string Slug { get; set; } = string.Empty;

        public string CacheKey => $"{nameof(ReadPlatformBySlugQuery)}_{Slug}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPlatformBySlugQuery()
        {
        }

        public ReadPlatformBySlugQuery(string slug)
        {
            Slug = slug;
        }
    }
}
