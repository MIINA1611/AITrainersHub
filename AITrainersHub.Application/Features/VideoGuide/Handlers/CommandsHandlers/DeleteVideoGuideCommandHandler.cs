using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.VideoGuide.Handlers;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteVideoGuideCommand.
    /// </summary>
    public class DeleteVideoGuideCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteVideoGuideCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteVideoGuideCommand request,
            CancellationToken cancellationToken)
        {
            var videoGuide = await context.VideoGuides
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (videoGuide == null)
            {
                throw new ResultException($"Video guide with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == videoGuide.PlatformId, cancellationToken);

            context.VideoGuides.Remove(videoGuide);
            await context.SaveChangesAsync(cancellationToken);

            VideoGuideCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, videoGuide.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadVideoGuideByIdQuery)}_{request.Id}");
            request.CacheKeySet.Add(nameof(ReadPendingVideoGuidesQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{videoGuide.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return true;
        }
    }
}
