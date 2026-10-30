using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Commands
{
    /// <summary>
    /// Represents a command to delete a work opportunity.
    /// </summary>
    public class DeleteWorkOpportunityCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadWorkOpportunitiesQuery),
            nameof(ReadPendingWorkOpportunitiesQuery)
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteWorkOpportunityCommand()
        {
        }

        public DeleteWorkOpportunityCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadWorkOpportunityByIdQuery)}_{id}");
        }
    }
}
