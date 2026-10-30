using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Handlers;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoGuideEntity = AITrainersHub.Domain.Models.VideoGuide;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateVideoGuideCommand, including approving or rejecting a video guide.
    /// </summary>
    public class UpdateVideoGuideCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateVideoGuideCommand, VideoGuideDto>
    {
        public async Task<VideoGuideDto> Handle(
            UpdateVideoGuideCommand request,
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

            videoGuide.Update(
                request.Title,
                request.Language,
                request.Url,
                request.SourceType,
                request.AuthorOrChannel,
                request.PublishedAt,
                request.IsApproved);

            context.VideoGuides.Update(videoGuide);
            await context.SaveChangesAsync(cancellationToken);

            VideoGuideCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, videoGuide.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadVideoGuideByIdQuery)}_{videoGuide.Id}");
            request.CacheKeySet.Add(nameof(ReadPendingVideoGuidesQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{videoGuide.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return Map(videoGuide);
        }

        private static VideoGuideDto Map(VideoGuideEntity videoGuide) => new()
        {
            Id = videoGuide.Id,
            PlatformId = videoGuide.PlatformId,
            Title = videoGuide.Title,
            Language = videoGuide.Language,
            Url = videoGuide.Url,
            SourceType = videoGuide.SourceType,
            AuthorOrChannel = videoGuide.AuthorOrChannel,
            PublishedAt = videoGuide.PublishedAt,
            IsApproved = videoGuide.IsApproved,
            CreatedAt = videoGuide.CreatedAt
        };
    }
}
