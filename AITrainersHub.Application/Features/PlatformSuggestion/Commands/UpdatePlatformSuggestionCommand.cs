using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Commands
{
    /// <summary>
    /// Represents a command to update an existing platform suggestion.
    /// </summary>
    public class UpdatePlatformSuggestionCommand : IRequest<PlatformSuggestionDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? PlatformName { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public string? EmailOfUser { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsReviewed { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadPlatformSuggestionsQuery),
            $"{nameof(ReadPlatformSuggestionsQuery)}_reviewed",
            $"{nameof(ReadPlatformSuggestionsQuery)}_pending"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdatePlatformSuggestionCommand()
        {
        }

        public UpdatePlatformSuggestionCommand(
            Guid id,
            string? platformName,
            string? url,
            string? description,
            string? emailOfUser,
            Guid? categoryId,
            bool? isReviewed)
        {
            Id = id;
            PlatformName = platformName;
            Url = url;
            Description = description;
            EmailOfUser = emailOfUser;
            CategoryId = categoryId;
            IsReviewed = isReviewed;
            CacheKeySet.Add($"{nameof(ReadPlatformSuggestionByIdQuery)}_{id}");
        }
    }
}
