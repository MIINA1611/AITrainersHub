using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewEntity = AITrainersHub.Domain.Models.Review;

namespace AITrainersHub.Application.Features.Review.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateReviewCommand, including approving or rejecting a review.
    /// </summary>
    public class UpdateReviewCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateReviewCommand, ReviewDto>
    {
        public async Task<ReviewDto> Handle(
            UpdateReviewCommand request,
            CancellationToken cancellationToken)
        {
            var review = await context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null)
            {
                throw new ResultException($"Review with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == review.PlatformId, cancellationToken);

            review.Update(
                request.ReviewerName,
                request.Rating,
                request.Comment,
                request.IsApproved);

            context.Reviews.Update(review);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, review, platform?.Slug);

            return Map(review);
        }

        private static void AddCacheKeys(UpdateReviewCommand request, ReviewEntity review, string? platformSlug)
        {
            request.CacheKeySet.Add($"{nameof(ReadReviewByIdQuery)}_{review.Id}");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_all");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_approved");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_pending");
            request.CacheKeySet.Add(nameof(ReadPendingReviewsQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{review.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (!string.IsNullOrWhiteSpace(platformSlug))
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platformSlug}");
            }
        }

        private static ReviewDto Map(ReviewEntity review) => new()
        {
            Id = review.Id,
            PlatformId = review.PlatformId,
            ReviewerName = review.ReviewerName,
            Rating = review.Rating,
            Comment = review.Comment,
            IsApproved = review.IsApproved,
            CreatedAt = review.CreatedAt
        };
    }
}
