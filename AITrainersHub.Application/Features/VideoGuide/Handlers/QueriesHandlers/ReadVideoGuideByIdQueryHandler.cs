using System.Net;
using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.VideoGuide.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadVideoGuideByIdQuery.
    /// </summary>
    public class ReadVideoGuideByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadVideoGuideByIdQuery, VideoGuideDto>
    {
        public async Task<VideoGuideDto> Handle(
            ReadVideoGuideByIdQuery request,
            CancellationToken cancellationToken)
        {
            var videoGuide = await context.VideoGuides
                .AsNoTracking()
                .Where(v => v.Id == request.Id)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (videoGuide == null)
            {
                throw new ResultException($"Video guide with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return videoGuide;
        }
    }
}
