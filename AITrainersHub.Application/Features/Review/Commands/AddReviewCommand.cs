using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Commands
{
    /// <summary>
    /// Represents a command to add a new review to a platform.
    /// </summary>
    public class AddReviewCommand : IRequest<ReviewDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddReviewCommand()
        {
        }

        public AddReviewCommand(Guid platformId, string reviewerName, int rating, string comment)
        {
            PlatformId = platformId;
            ReviewerName = reviewerName;
            Rating = rating;
            Comment = comment;
        }
    }
}
