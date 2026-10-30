using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Advertisement.Commands
{
    /// <summary>
    /// Represents a command to delete an advertisement.
    /// </summary>
    public class DeleteAdvertisementCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadAdvertisementsQuery),
            $"{nameof(ReadAdvertisementsQuery)}_active",
            $"{nameof(ReadAdvertisementsQuery)}_inactive"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteAdvertisementCommand()
        {
        }

        public DeleteAdvertisementCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadAdvertisementByIdQuery)}_{id}");
        }
    }
}
