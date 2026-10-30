using System.Net;
using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePlatformSuggestionCommand.
    /// </summary>
    public class DeletePlatformSuggestionCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePlatformSuggestionCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePlatformSuggestionCommand request,
            CancellationToken cancellationToken)
        {
            var platformSuggestion = await context.PlatformSuggestions
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (platformSuggestion == null)
            {
                throw new ResultException($"Platform suggestion with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.PlatformSuggestions.Remove(platformSuggestion);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPlatformSuggestionByIdQuery)}_{platformSuggestion.Id}");

            return true;
        }
    }
}
