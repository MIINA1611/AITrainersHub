using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using WorkOpportunityEntity = AITrainersHub.Domain.Models.WorkOpportunity;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddWorkOpportunityCommand and persists a new work opportunity.
    /// </summary>
    public class AddWorkOpportunityCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddWorkOpportunityCommand, WorkOpportunityDto>
    {
        public async Task<WorkOpportunityDto> Handle(
            AddWorkOpportunityCommand request,
            CancellationToken cancellationToken)
        {
            var workOpportunity = new WorkOpportunityEntity(
                request.LeaderId,
                request.Title,
                request.DescriptionAr,
                request.DescriptionEn,
                request.WhatsAppLink);

            await context.WorkOpportunities.AddAsync(workOpportunity, cancellationToken);
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
