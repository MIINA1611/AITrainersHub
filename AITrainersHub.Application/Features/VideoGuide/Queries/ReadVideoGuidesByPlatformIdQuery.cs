using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Enums;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Queries
{
    /// <summary>
    /// Represents a query to read the video guides of a platform, optionally filtered by language.
    /// </summary>
    public class ReadVideoGuidesByPlatformIdQuery : IRequest<List<VideoGuideDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }
        public GuideLanguage? Language { get; set; }

        public string CacheKey => Language.HasValue
            ? $"{nameof(ReadVideoGuidesByPlatformIdQuery)}_{PlatformId}_{Language.Value}"
            : $"{nameof(ReadVideoGuidesByPlatformIdQuery)}_{PlatformId}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadVideoGuidesByPlatformIdQuery()
        {
        }

        public ReadVideoGuidesByPlatformIdQuery(Guid platformId, GuideLanguage? language = null)
        {
            PlatformId = platformId;
            Language = language;
        }
    }
}
