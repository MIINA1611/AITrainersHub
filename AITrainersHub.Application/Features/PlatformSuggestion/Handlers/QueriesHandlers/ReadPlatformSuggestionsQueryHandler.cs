using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformSuggestionEntity = AITrainersHub.Domain.Models.PlatformSuggestion;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPlatformSuggestionsQuery and applies the optional review-state filter.
    /// </summary>
    public class ReadPlatformSuggestionsQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPlatformSuggestionsQuery, List<PlatformSuggestionDto>>
    {
        public async Task<List<PlatformSuggestionDto>> Handle(
            ReadPlatformSuggestionsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<PlatformSuggestionEntity> query = context.PlatformSuggestions.AsNoTracking();

            if (request.IsReviewed.HasValue)
            {
                query = query.Where(s => s.IsReviewed == request.IsReviewed.Value);
            }

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new PlatformSuggestionDto
                {
                    Id = s.Id,
                    PlatformName = s.PlatformName,
                    Url = s.Url,
                    Description = s.Description,
                    EmailOfUser = s.EmailOfUser,
                    CategoryId = s.CategoryId,
                    IsReviewed = s.IsReviewed,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
