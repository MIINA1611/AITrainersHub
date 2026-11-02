using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOpportunityEntity = AITrainersHub.Domain.Models.WorkOpportunity;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadWorkOpportunitiesQuery and applies the optional filters.
    /// </summary>
    public class ReadWorkOpportunitiesQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadWorkOpportunitiesQuery, List<WorkOpportunityDto>>
    {
        public async Task<List<WorkOpportunityDto>> Handle(
            ReadWorkOpportunitiesQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<WorkOpportunityEntity> query = context.WorkOpportunities.AsNoTracking();

            if (request.LeaderId.HasValue)
            {
                query = query.Where(o => o.LeaderId == request.LeaderId.Value);
            }

            if (request.IsApproved.HasValue)
            {
                query = query.Where(o => o.IsApproved == request.IsApproved.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(o => o.IsActive == request.IsActive.Value);
            }

            return await query
                .OrderByDescending(o => o.CreatedAt)
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
