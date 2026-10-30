using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Commands
{
    /// <summary>
    /// Represents a command to update an existing work opportunity.
    /// </summary>
    public class UpdateWorkOpportunityCommand : IRequest<WorkOpportunityDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? WhatsAppLink { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsActive { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadWorkOpportunitiesQuery),
            nameof(ReadPendingWorkOpportunitiesQuery)
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateWorkOpportunityCommand()
        {
        }

        public UpdateWorkOpportunityCommand(
            Guid id,
            string? title,
            string? descriptionAr,
            string? descriptionEn,
            string? whatsAppLink,
            bool? isApproved,
            bool? isActive)
        {
            Id = id;
            Title = title;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WhatsAppLink = whatsAppLink;
            IsApproved = isApproved;
            IsActive = isActive;
            CacheKeySet.Add($"{nameof(ReadWorkOpportunityByIdQuery)}_{id}");
        }
    }
}
