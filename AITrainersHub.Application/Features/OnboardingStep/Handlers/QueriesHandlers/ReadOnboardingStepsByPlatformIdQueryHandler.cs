using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.OnboardingStep.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadOnboardingStepsByPlatformIdQuery.
    /// </summary>
    public class ReadOnboardingStepsByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadOnboardingStepsByPlatformIdQuery, List<OnboardingStepDto>>
    {
        public async Task<List<OnboardingStepDto>> Handle(
            ReadOnboardingStepsByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            return await context.OnboardingSteps
                .AsNoTracking()
                .Where(s => s.PlatformId == request.PlatformId)
                .OrderBy(s => s.StepNumber)
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
                .ToListAsync(cancellationToken);
        }
    }
}
