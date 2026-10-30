using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Domain.Enums;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers
{
    /// <summary>
    /// Centralizes the cache keys invalidated by video guide commands.
    /// </summary>
    internal static class VideoGuideCacheKeys
    {
        public static void AddPlatformCacheKeys(HashSet<string> cacheKeys, Guid platformId)
        {
            cacheKeys.Add($"{nameof(ReadVideoGuidesByPlatformIdQuery)}_{platformId}");
            cacheKeys.Add($"{nameof(ReadVideoGuidesByPlatformIdQuery)}_{platformId}_{GuideLanguage.Arabic}");
            cacheKeys.Add($"{nameof(ReadVideoGuidesByPlatformIdQuery)}_{platformId}_{GuideLanguage.English}");
        }
    }
}
