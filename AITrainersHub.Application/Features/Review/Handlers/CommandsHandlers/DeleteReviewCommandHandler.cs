using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Review.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteReviewCommand.
    /// </summary>
    public class DeleteReviewCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteReviewCommand request,
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

            context.Reviews.Remove(review);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadReviewByIdQuery)}_{request.Id}");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_all");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_approved");
            request.CacheKeySet.Add($"{nameof(ReadReviewsByPlatformIdQuery)}_{review.PlatformId}_pending");
            request.CacheKeySet.Add(nameof(ReadPendingReviewsQuery));
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{review.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return true;
        }
    }
}
