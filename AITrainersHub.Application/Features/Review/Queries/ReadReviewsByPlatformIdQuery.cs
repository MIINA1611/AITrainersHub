using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Queries
{
    /// <summary>
    /// Represents a query to read the reviews of a platform, optionally filtered by approval state.
    /// </summary>
    public class ReadReviewsByPlatformIdQuery : IRequest<List<ReviewDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }
        public bool? IsApproved { get; set; }

        public string CacheKey =>
            $"{nameof(ReadReviewsByPlatformIdQuery)}_{PlatformId}_{(IsApproved.HasValue ? (IsApproved.Value ? "approved" : "pending") : "all")}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadReviewsByPlatformIdQuery()
        {
        }

        public ReadReviewsByPlatformIdQuery(Guid platformId, bool? isApproved = null)
        {
            PlatformId = platformId;
            IsApproved = isApproved;
        }
    }
}
