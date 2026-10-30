using System.Net;
using AITrainersHub.Application.Features.OnboardingStep.Commands;
using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.OnboardingStep.Handlers;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnboardingStepEntity = AITrainersHub.Domain.Models.OnboardingStep;

namespace AITrainersHub.Application.Features.OnboardingStep.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateOnboardingStepCommand.
    /// </summary>
    public class UpdateOnboardingStepCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateOnboardingStepCommand, OnboardingStepDto>
    {
        public async Task<OnboardingStepDto> Handle(
            UpdateOnboardingStepCommand request,
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

            onboardingStep.Update(
                request.StepNumber,
                request.TitleAr,
                request.TitleEn,
                request.DescriptionAr,
                request.DescriptionEn,
                request.Tip,
                request.ScreenshotUrl);

            context.OnboardingSteps.Update(onboardingStep);
            await context.SaveChangesAsync(cancellationToken);

            OnboardingStepCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, onboardingStep.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadOnboardingStepByIdQuery)}_{onboardingStep.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{onboardingStep.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return Map(onboardingStep);
        }

        private static OnboardingStepDto Map(OnboardingStepEntity onboardingStep) => new()
        {
            Id = onboardingStep.Id,
            PlatformId = onboardingStep.PlatformId,
            StepNumber = onboardingStep.StepNumber,
            TitleAr = onboardingStep.TitleAr,
            TitleEn = onboardingStep.TitleEn,
            DescriptionAr = onboardingStep.DescriptionAr,
            DescriptionEn = onboardingStep.DescriptionEn,
            Tip = onboardingStep.Tip,
            ScreenshotUrl = onboardingStep.ScreenshotUrl
        };
    }
}
