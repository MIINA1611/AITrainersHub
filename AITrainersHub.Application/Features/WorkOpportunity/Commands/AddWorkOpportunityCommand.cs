using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Commands
{
    /// <summary>
    /// Represents a command to create a new work opportunity.
    /// </summary>
    public class AddWorkOpportunityCommand : IRequest<WorkOpportunityDto>, ICacheInvalidatorCommand
    {
        public Guid LeaderId { get; set; }
        public Guid PlatformId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WhatsAppLink { get; set; } = string.Empty;

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadWorkOpportunitiesQuery),
            nameof(ReadPendingWorkOpportunitiesQuery)
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddWorkOpportunityCommand()
        {
        }

        public AddWorkOpportunityCommand(
            Guid leaderId,
            Guid platformId,
            string title,
            string descriptionAr,
            string descriptionEn,
            string whatsAppLink)
        {
            LeaderId = leaderId;
            PlatformId = platformId;
            Title = title;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WhatsAppLink = whatsAppLink;
        }
    }
}
