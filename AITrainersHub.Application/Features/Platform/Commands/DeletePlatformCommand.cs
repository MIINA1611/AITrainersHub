using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Platform.Commands
{
    /// <summary>
    /// Represents a command to delete a platform by its unique identifier.
    /// </summary>
    public class DeletePlatformCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPlatformsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePlatformCommand()
        {
        }

        public DeletePlatformCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{id}");
        }
    }
}
