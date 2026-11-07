using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PlatformSuggestionEntity = AITrainersHub.Domain.Models.PlatformSuggestion;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPlatformSuggestionCommand and persists a new suggestion.
    /// </summary>
    public class AddPlatformSuggestionCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPlatformSuggestionCommand, PlatformSuggestionDto>
    {
        public async Task<PlatformSuggestionDto> Handle(
            AddPlatformSuggestionCommand request,
            CancellationToken cancellationToken)
        {
            var platformSuggestion = new PlatformSuggestionEntity(
                request.PlatformName,
                request.Url,
                request.Description,
                request.SubmitterName,
                request.CategoryId);

            await context.PlatformSuggestions.AddAsync(platformSuggestion, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPlatformSuggestionByIdQuery)}_{platformSuggestion.Id}");

            return Map(platformSuggestion);
        }

        private static PlatformSuggestionDto Map(PlatformSuggestionEntity platformSuggestion) => new()
        {
            Id = platformSuggestion.Id,
            PlatformName = platformSuggestion.PlatformName,
            Url = platformSuggestion.Url,
            Description = platformSuggestion.Description,
            SubmitterName = platformSuggestion.SubmitterName,
            EmailOfUser = platformSuggestion.EmailOfUser,
            CategoryId = platformSuggestion.CategoryId,
            IsReviewed = platformSuggestion.IsReviewed,
            CreatedAt = platformSuggestion.CreatedAt
        };
    }
}
