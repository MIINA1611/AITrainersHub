using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Review.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadReviewsByPlatformIdQuery.
    /// </summary>
    public class ReadReviewsByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadReviewsByPlatformIdQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(
            ReadReviewsByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.Reviews
                .AsNoTracking()
                .Where(r => r.PlatformId == request.PlatformId);

            if (request.IsApproved.HasValue)
            {
                query = query.Where(r => r.IsApproved == request.IsApproved.Value);
            }

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    PlatformId = r.PlatformId,
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    IsApproved = r.IsApproved,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
