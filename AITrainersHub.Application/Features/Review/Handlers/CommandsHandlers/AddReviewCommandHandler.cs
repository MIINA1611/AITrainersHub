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
    /// Handles the AddReviewCommand and persists a new pending review.
    /// </summary>
    public class AddReviewCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddReviewCommand, ReviewDto>
    {
        public async Task<ReviewDto> Handle(
            AddReviewCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var review = new ReviewEntity(
                request.PlatformId,
                request.ReviewerName,
                request.Rating,
                request.Comment);

            await context.Reviews.AddAsync(review, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, review, platform.Slug);

            return Map(review);
        }

        private static void AddCacheKeys(AddReviewCommand request, ReviewEntity review, string platformSlug)
        {
            request.CacheKeySet.Add($"{nameof(ReadReviewByIdQuery)}_{review.Id}");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_all");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_approved");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_pending");
            request.CacheKeySet.Add(nameof(ReadPendingReviewsQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{review.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platformSlug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
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
