using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.OnboardingStep.Queries
{
    /// <summary>
    /// Represents a query to read an onboarding step by its unique identifier.
    /// </summary>
    public class ReadOnboardingStepByIdQuery : IRequest<OnboardingStepDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadOnboardingStepByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadOnboardingStepByIdQuery()
        {
        }

        public ReadOnboardingStepByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
