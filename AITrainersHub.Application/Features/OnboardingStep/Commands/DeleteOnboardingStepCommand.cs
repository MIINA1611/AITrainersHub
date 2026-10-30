using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.OnboardingStep.Commands
{
    /// <summary>
    /// Represents a command to delete an onboarding step.
    /// </summary>
    public class DeleteOnboardingStepCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteOnboardingStepCommand()
        {
        }

        public DeleteOnboardingStepCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadOnboardingStepByIdQuery)}_{id}");
        }
    }
}
