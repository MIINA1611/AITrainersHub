using System.Net;
using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOpportunityEntity = AITrainersHub.Domain.Models.WorkOpportunity;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateWorkOpportunityCommand and applies the supplied values.
    /// </summary>
    public class UpdateWorkOpportunityCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateWorkOpportunityCommand, WorkOpportunityDto>
    {
        public async Task<WorkOpportunityDto> Handle(
            UpdateWorkOpportunityCommand request,
            CancellationToken cancellationToken)
        {
            var workOpportunity = await context.WorkOpportunities
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (workOpportunity == null)
            {
                throw new ResultException($"Work opportunity with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            workOpportunity.Update(
                request.Title,
                request.DescriptionAr,
                request.DescriptionEn,
                request.WhatsAppLink,
                request.IsApproved,
                request.IsActive);

            context.WorkOpportunities.Update(workOpportunity);
            await context.SaveChangesAsync(cancellationToken);

            WorkOpportunityCacheKeys.AddListCacheKeys(request.CacheKeySet, workOpportunity.LeaderId);
            request.CacheKeySet.Add($"{nameof(ReadWorkOpportunityByIdQuery)}_{workOpportunity.Id}");

            return Map(workOpportunity);
        }

        private static WorkOpportunityDto Map(WorkOpportunityEntity workOpportunity) => new()
        {
            Id = workOpportunity.Id,
            LeaderId = workOpportunity.LeaderId,
            Title = workOpportunity.Title,
            DescriptionAr = workOpportunity.DescriptionAr,
            DescriptionEn = workOpportunity.DescriptionEn,
            WhatsAppLink = workOpportunity.WhatsAppLink,
            IsApproved = workOpportunity.IsApproved,
            IsActive = workOpportunity.IsActive,
            CreatedAt = workOpportunity.CreatedAt
        };
    }
}
