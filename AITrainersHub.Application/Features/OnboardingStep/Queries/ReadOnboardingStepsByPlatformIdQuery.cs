using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.OnboardingStep.Queries
{
    /// <summary>
    /// Represents a query to read all onboarding steps of a platform ordered by step number.
    /// </summary>
    public class ReadOnboardingStepsByPlatformIdQuery : IRequest<List<OnboardingStepDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }

        public string CacheKey => $"{nameof(ReadOnboardingStepsByPlatformIdQuery)}_{PlatformId}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadOnboardingStepsByPlatformIdQuery()
        {
        }

        public ReadOnboardingStepsByPlatformIdQuery(Guid platformId)
        {
            PlatformId = platformId;
        }
    }
}
