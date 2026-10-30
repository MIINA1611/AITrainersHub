using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Queries
{
    /// <summary>
    /// Represents a query to read a review by its unique identifier.
    /// </summary>
    public class ReadReviewByIdQuery : IRequest<ReviewDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadReviewByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadReviewByIdQuery()
        {
        }

        public ReadReviewByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
