using System.Net;
using AITrainersHub.Application.Features.OnboardingStep.Commands;
using AITrainersHub.Application.Features.OnboardingStep.Handlers;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.OnboardingStep.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteOnboardingStepCommand.
    /// </summary>
    public class DeleteOnboardingStepCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteOnboardingStepCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteOnboardingStepCommand request,
            CancellationToken cancellationToken)
        {
            var onboardingStep = await context.OnboardingSteps
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (onboardingStep == null)
            {
                throw new ResultException($"Onboarding step with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == onboardingStep.PlatformId, cancellationToken);

            context.OnboardingSteps.Remove(onboardingStep);
            await context.SaveChangesAsync(cancellationToken);

            OnboardingStepCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, onboardingStep.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadOnboardingStepByIdQuery)}_{request.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{onboardingStep.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return true;
        }
    }
}
