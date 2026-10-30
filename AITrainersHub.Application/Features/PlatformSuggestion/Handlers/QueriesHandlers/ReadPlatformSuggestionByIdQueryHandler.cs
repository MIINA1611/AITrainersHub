using System.Net;
using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPlatformSuggestionByIdQuery.
    /// </summary>
    public class ReadPlatformSuggestionByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPlatformSuggestionByIdQuery, PlatformSuggestionDto>
    {
        public async Task<PlatformSuggestionDto> Handle(
            ReadPlatformSuggestionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var platformSuggestion = await context.PlatformSuggestions
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (platformSuggestion == null)
            {
                throw new ResultException($"Platform suggestion with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return platformSuggestion;
        }
    }
}
