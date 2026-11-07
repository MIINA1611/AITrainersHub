using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Features.VideoGuide.Handlers;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoGuideEntity = AITrainersHub.Domain.Models.VideoGuide;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddVideoGuideCommand and persists a new video guide.
    /// Admin-created guides are marked as approved immediately.
    /// </summary>
    public class AddVideoGuideCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddVideoGuideCommand, VideoGuideDto>
    {
        public async Task<VideoGuideDto> Handle(
            AddVideoGuideCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var videoGuide = new VideoGuideEntity(
                request.PlatformId,
                request.Title,
                request.Language,
                request.Url,
                request.SourceType,
                request.AuthorOrChannel,
                request.PublishedAt)
            {
                IsApproved = request.IsApproved
            };

            await context.VideoGuides.AddAsync(videoGuide, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, videoGuide, platform.Slug);

            return Map(videoGuide);
        }

        private static void AddCacheKeys(AddVideoGuideCommand request, VideoGuideEntity videoGuide, string platformSlug)
        {
            VideoGuideCacheKeys.AddPlatformCacheKeys(request.CacheKeySet, videoGuide.PlatformId);
            request.CacheKeySet.Add($"{nameof(ReadVideoGuideByIdQuery)}_{videoGuide.Id}");
            request.CacheKeySet.Add(nameof(ReadPendingVideoGuidesQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{videoGuide.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platformSlug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
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
