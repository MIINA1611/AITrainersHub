using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Review.Commands
{
    /// <summary>
    /// Represents a command to delete a review.
    /// </summary>
    public class DeleteReviewCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteReviewCommand()
        {
        }

        public DeleteReviewCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadReviewByIdQuery)}_{id}");
        }
    }
}
