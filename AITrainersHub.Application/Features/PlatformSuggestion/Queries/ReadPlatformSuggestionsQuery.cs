using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Queries
{
    /// <summary>
    /// Represents a query to read platform suggestions, optionally filtered by review state.
    /// </summary>
    public class ReadPlatformSuggestionsQuery : IRequest<List<PlatformSuggestionDto>>, ICacheableQuery
    {
        public bool? IsReviewed { get; set; }

        public string CacheKey =>
            !IsReviewed.HasValue
                ? nameof(ReadPlatformSuggestionsQuery)
                : $"{nameof(ReadPlatformSuggestionsQuery)}_{(IsReviewed.Value ? "reviewed" : "pending")}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPlatformSuggestionsQuery()
        {
        }

        public ReadPlatformSuggestionsQuery(bool? isReviewed = null)
        {
            IsReviewed = isReviewed;
        }
    }
}
