using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Queries
{
    /// <summary>
    /// Represents a query to read a platform suggestion by its unique identifier.
    /// </summary>
    public class ReadPlatformSuggestionByIdQuery : IRequest<PlatformSuggestionDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadPlatformSuggestionByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPlatformSuggestionByIdQuery()
        {
        }

        public ReadPlatformSuggestionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
