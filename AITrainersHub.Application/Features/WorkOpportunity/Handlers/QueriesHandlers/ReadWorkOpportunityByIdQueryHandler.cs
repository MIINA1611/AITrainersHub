using System.Net;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadWorkOpportunityByIdQuery.
    /// </summary>
    public class ReadWorkOpportunityByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadWorkOpportunityByIdQuery, WorkOpportunityDto>
    {
        public async Task<WorkOpportunityDto> Handle(
            ReadWorkOpportunityByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workOpportunity = await context.WorkOpportunities
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new WorkOpportunityDto
                {
                    Id = o.Id,
                    LeaderId = o.LeaderId,
                    Title = o.Title,
                    DescriptionAr = o.DescriptionAr,
                    DescriptionEn = o.DescriptionEn,
                    WhatsAppLink = o.WhatsAppLink,
                    IsApproved = o.IsApproved,
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (workOpportunity == null)
            {
                throw new ResultException($"Work opportunity with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return workOpportunity;
        }
    }
}
