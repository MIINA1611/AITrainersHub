using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPendingWorkOpportunitiesQuery.
    /// </summary>
    public class ReadPendingWorkOpportunitiesQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPendingWorkOpportunitiesQuery, List<WorkOpportunityDto>>
    {
        public async Task<List<WorkOpportunityDto>> Handle(
            ReadPendingWorkOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            return await context.WorkOpportunities
                .AsNoTracking()
                .Where(o => !o.IsApproved)
                .OrderBy(o => o.CreatedAt)
                .Select(o => new WorkOpportunityDto
                {
                    Id = o.Id,
                    LeaderId = o.LeaderId,
                    LeaderDisplayName = o.Leader.DisplayName ?? o.Leader.UserName ?? string.Empty,
                    PlatformId = o.PlatformId,
                    PlatformName = o.Platform.Name,
                    PlatformSlug = o.Platform.Slug,
                    Title = o.Title,
                    DescriptionAr = o.DescriptionAr,
                    DescriptionEn = o.DescriptionEn,
                    WhatsAppLink = o.WhatsAppLink,
                    IsApproved = o.IsApproved,
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
