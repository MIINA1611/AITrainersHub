using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPendingVideoGuidesQuery.
    /// </summary>
    public class ReadPendingVideoGuidesQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPendingVideoGuidesQuery, List<VideoGuideDto>>
    {
        public async Task<List<VideoGuideDto>> Handle(
            ReadPendingVideoGuidesQuery request,
            CancellationToken cancellationToken)
        {
            return await context.VideoGuides
                .AsNoTracking()
                .Where(v => !v.IsApproved)
                .OrderBy(v => v.CreatedAt)
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
