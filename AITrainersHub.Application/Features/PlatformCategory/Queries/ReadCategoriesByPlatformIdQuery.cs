using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCategory.Queries
{
    /// <summary>
    /// Represents a query to read all categories linked to a platform.
    /// </summary>
    public class ReadCategoriesByPlatformIdQuery : IRequest<List<PlatformCategoryDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }

        public string CacheKey => $"{nameof(ReadCategoriesByPlatformIdQuery)}_{PlatformId}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadCategoriesByPlatformIdQuery()
        {
        }

        public ReadCategoriesByPlatformIdQuery(Guid platformId)
        {
            PlatformId = platformId;
        }
    }
}
