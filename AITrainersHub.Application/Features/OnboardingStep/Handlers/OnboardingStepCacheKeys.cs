using AITrainersHub.Application.Features.OnboardingStep.Queries;

namespace AITrainersHub.Application.Features.OnboardingStep.Handlers
{
    /// <summary>
    /// Centralizes the cache keys invalidated by onboarding step commands.
    /// </summary>
    internal static class OnboardingStepCacheKeys
    {
        public static void AddPlatformCacheKeys(HashSet<string> cacheKeys, Guid platformId)
        {
            cacheKeys.Add($"{nameof(ReadOnboardingStepsByPlatformIdQuery)}_{platformId}");
        }
    }
}
