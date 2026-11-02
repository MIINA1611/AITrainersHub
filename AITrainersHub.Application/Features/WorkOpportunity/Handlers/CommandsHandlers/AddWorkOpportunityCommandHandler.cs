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
    /// Handles the AddWorkOpportunityCommand and persists a new work opportunity.
    /// </summary>
    public class AddWorkOpportunityCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddWorkOpportunityCommand, WorkOpportunityDto>
    {
        public async Task<WorkOpportunityDto> Handle(
            AddWorkOpportunityCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var leaderName = await context.Users
                .AsNoTracking()
                .Where(u => u.Id == request.LeaderId)
                .Select(u => u.DisplayName ?? u.UserName ?? string.Empty)
                .FirstOrDefaultAsync(cancellationToken);

            var workOpportunity = new WorkOpportunityEntity(
                request.LeaderId,
                request.PlatformId,
                request.Title,
                request.DescriptionAr,
                request.DescriptionEn,
                request.WhatsAppLink);

            await context.WorkOpportunities.AddAsync(workOpportunity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            WorkOpportunityCacheKeys.AddListCacheKeys(request.CacheKeySet, workOpportunity.LeaderId);
            request.CacheKeySet.Add($"{nameof(ReadWorkOpportunityByIdQuery)}_{workOpportunity.Id}");

            return Map(workOpportunity, platform.Name, platform.Slug, leaderName ?? string.Empty);
        }

        private static WorkOpportunityDto Map(
            WorkOpportunityEntity workOpportunity,
            string platformName,
            string platformSlug,
            string leaderDisplayName) => new()
        {
            Id = workOpportunity.Id,
            LeaderId = workOpportunity.LeaderId,
            LeaderDisplayName = leaderDisplayName,
            PlatformId = workOpportunity.PlatformId,
            PlatformName = platformName,
            PlatformSlug = platformSlug,
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
