using System.Net;
using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteWorkOpportunityCommand.
    /// </summary>
    public class DeleteWorkOpportunityCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteWorkOpportunityCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteWorkOpportunityCommand request,
            CancellationToken cancellationToken)
        {
            var workOpportunity = await context.WorkOpportunities
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (workOpportunity == null)
            {
                throw new ResultException($"Work opportunity with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.WorkOpportunities.Remove(workOpportunity);
            await context.SaveChangesAsync(cancellationToken);

            WorkOpportunityCacheKeys.AddListCacheKeys(request.CacheKeySet, workOpportunity.LeaderId);
            request.CacheKeySet.Add($"{nameof(ReadWorkOpportunityByIdQuery)}_{workOpportunity.Id}");

            return true;
        }
    }
}
