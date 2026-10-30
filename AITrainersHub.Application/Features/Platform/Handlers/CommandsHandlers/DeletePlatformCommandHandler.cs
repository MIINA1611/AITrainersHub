using System.Net;
using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Platform.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePlatformCommand and removes the platform.
    /// </summary>
    public class DeletePlatformCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePlatformCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePlatformCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            var slug = platform.Slug;

            context.Platforms.Remove(platform);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{slug}");

            return true;
        }
    }
}
