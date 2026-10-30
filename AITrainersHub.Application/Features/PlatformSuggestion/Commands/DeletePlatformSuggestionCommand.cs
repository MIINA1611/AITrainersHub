using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Commands
{
    /// <summary>
    /// Represents a command to delete a platform suggestion.
    /// </summary>
    public class DeletePlatformSuggestionCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadPlatformSuggestionsQuery),
            $"{nameof(ReadPlatformSuggestionsQuery)}_reviewed",
            $"{nameof(ReadPlatformSuggestionsQuery)}_pending"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePlatformSuggestionCommand()
        {
        }

        public DeletePlatformSuggestionCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadPlatformSuggestionByIdQuery)}_{id}");
        }
    }
}
