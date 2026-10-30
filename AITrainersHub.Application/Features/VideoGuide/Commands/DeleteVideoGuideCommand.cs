using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Commands
{
    /// <summary>
    /// Represents a command to delete a video guide.
    /// </summary>
    public class DeleteVideoGuideCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteVideoGuideCommand()
        {
        }

        public DeleteVideoGuideCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadVideoGuideByIdQuery)}_{id}");
        }
    }
}
