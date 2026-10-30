using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Commands
{
    /// <summary>
    /// Represents a command to update an existing review, including its approval state.
    /// </summary>
    public class UpdateReviewCommand : IRequest<ReviewDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? ReviewerName { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsApproved { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateReviewCommand()
        {
        }

        public UpdateReviewCommand(
            Guid id,
            string? reviewerName,
            int? rating,
            string? comment,
            bool? isApproved)
        {
            Id = id;
            ReviewerName = reviewerName;
            Rating = rating;
            Comment = comment;
            IsApproved = isApproved;
            CacheKeySet.Add($"{nameof(ReadReviewByIdQuery)}_{id}");
        }
    }
}
