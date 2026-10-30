using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadVideoGuidesByPlatformIdQuery.
    /// </summary>
    public class ReadVideoGuidesByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadVideoGuidesByPlatformIdQuery, List<VideoGuideDto>>
    {
        public async Task<List<VideoGuideDto>> Handle(
            ReadVideoGuidesByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.VideoGuides
                .AsNoTracking()
                .Where(v => v.PlatformId == request.PlatformId);

            if (request.Language.HasValue)
            {
                query = query.Where(v => v.Language == request.Language.Value);
            }

            return await query
                .OrderByDescending(v => v.CreatedAt)
                .Select(v => new VideoGuideDto
                {
                    Id = v.Id,
                    PlatformId = v.PlatformId,
                    Title = v.Title,
                    Language = v.Language,
                    Url = v.Url,
                    SourceType = v.SourceType,
                    AuthorOrChannel = v.AuthorOrChannel,
                    PublishedAt = v.PublishedAt,
                    IsApproved = v.IsApproved,
                    CreatedAt = v.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
