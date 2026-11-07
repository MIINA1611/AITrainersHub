using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Commands
{
    /// <summary>
    /// Represents a command to create a new platform suggestion.
    /// </summary>
    public class AddPlatformSuggestionCommand : IRequest<PlatformSuggestionDto>, ICacheInvalidatorCommand
    {
        public string PlatformName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SubmitterName { get; set; } = string.Empty;
        public string? EmailOfUser { get; set; }
        public Guid? CategoryId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadPlatformSuggestionsQuery),
            $"{nameof(ReadPlatformSuggestionsQuery)}_reviewed",
            $"{nameof(ReadPlatformSuggestionsQuery)}_pending"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPlatformSuggestionCommand()
        {
        }

        public AddPlatformSuggestionCommand(
            string platformName,
            string url,
            string description,
            string submitterName,
            Guid? categoryId)
        {
            PlatformName = platformName;
            Url = url;
            Description = description;
            SubmitterName = submitterName;
            CategoryId = categoryId;
        }
    }
}
