using System.Net;
using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Review.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadReviewByIdQuery.
    /// </summary>
    public class ReadReviewByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadReviewByIdQuery, ReviewDto>
    {
        public async Task<ReviewDto> Handle(
            ReadReviewByIdQuery request,
            CancellationToken cancellationToken)
        {
            var review = await context.Reviews
                .AsNoTracking()
                .Where(r => r.Id == request.Id)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (review == null)
            {
                throw new ResultException($"Review with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return review;
        }
    }
}
