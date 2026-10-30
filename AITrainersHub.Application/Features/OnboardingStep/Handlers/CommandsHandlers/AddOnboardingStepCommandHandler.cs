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
    /// Handles the AddOnboardingStepCommand.
    /// </summary>
    public class AddOnboardingStepCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddOnboardingStepCommand, OnboardingStepDto>
    {
        public async Task<OnboardingStepDto> Handle(
            AddOnboardingStepCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var onboardingStep = new OnboardingStepEntity(
                request.PlatformId,
                request.StepNumber,
                request.TitleAr,
                request.TitleEn,
                request.DescriptionAr,
                request.DescriptionEn,
                request.Tip,
                request.ScreenshotUrl);

            await context.OnboardingSteps.AddAsync(onboardingStep, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, onboardingStep, platform.Slug);

            return Map(onboardingStep);
        }

        private static void AddCacheKeys(AddOnboardingStepCommand request, OnboardingStepEntity onboardingStep, string platformSlug)
        {
            OnboardingStepCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, onboardingStep.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadOnboardingStepByIdQuery)}_{onboardingStep.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{onboardingStep.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platformSlug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
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
