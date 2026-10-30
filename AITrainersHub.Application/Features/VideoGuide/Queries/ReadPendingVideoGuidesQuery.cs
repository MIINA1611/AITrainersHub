using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Queries
{
    /// <summary>
    /// Represents a query to read all video guides awaiting approval.
    /// </summary>
    public class ReadPendingVideoGuidesQuery : IRequest<List<VideoGuideDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(ReadPendingVideoGuidesQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
