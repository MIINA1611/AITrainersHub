using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Review.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPendingReviewsQuery.
    /// </summary>
    public class ReadPendingReviewsQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPendingReviewsQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(
            ReadPendingReviewsQuery request,
            CancellationToken cancellationToken)
        {
            return await context.Reviews
                .AsNoTracking()
                .Where(r => !r.IsApproved)
                .OrderBy(r => r.CreatedAt)
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
