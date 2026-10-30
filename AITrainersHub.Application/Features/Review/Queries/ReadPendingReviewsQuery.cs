using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Queries
{
    /// <summary>
    /// Represents a query to read all reviews awaiting approval.
    /// </summary>
    public class ReadPendingReviewsQuery : IRequest<List<ReviewDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(ReadPendingReviewsQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
