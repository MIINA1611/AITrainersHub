using System.Net;
using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.OnboardingStep.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadOnboardingStepByIdQuery.
    /// </summary>
    public class ReadOnboardingStepByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadOnboardingStepByIdQuery, OnboardingStepDto>
    {
        public async Task<OnboardingStepDto> Handle(
            ReadOnboardingStepByIdQuery request,
            CancellationToken cancellationToken)
        {
            var onboardingStep = await context.OnboardingSteps
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .Select(s => new OnboardingStepDto
                {
                    Id = s.Id,
                    PlatformId = s.PlatformId,
                    StepNumber = s.StepNumber,
                    TitleAr = s.TitleAr,
                    TitleEn = s.TitleEn,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    Tip = s.Tip,
                    ScreenshotUrl = s.ScreenshotUrl
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (onboardingStep == null)
            {
                throw new ResultException($"Onboarding step with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return onboardingStep;
        }
    }
}
