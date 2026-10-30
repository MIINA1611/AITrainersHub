using System.Net;
using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Features.PlatformSuggestion.DTOs;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformSuggestionEntity = AITrainersHub.Domain.Models.PlatformSuggestion;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdatePlatformSuggestionCommand and applies the supplied values.
    /// </summary>
    public class UpdatePlatformSuggestionCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdatePlatformSuggestionCommand, PlatformSuggestionDto>
    {
        public async Task<PlatformSuggestionDto> Handle(
            UpdatePlatformSuggestionCommand request,
            CancellationToken cancellationToken)
        {
            var platformSuggestion = await context.PlatformSuggestions
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (platformSuggestion == null)
            {
                throw new ResultException($"Platform suggestion with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            platformSuggestion.Update(
                request.PlatformName,
                request.Url,
                request.Description,
                request.EmailOfUser,
                request.CategoryId,
                request.IsReviewed);

            context.PlatformSuggestions.Update(platformSuggestion);
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
            EmailOfUser = platformSuggestion.EmailOfUser,
            CategoryId = platformSuggestion.CategoryId,
            IsReviewed = platformSuggestion.IsReviewed,
            CreatedAt = platformSuggestion.CreatedAt
        };
    }
}
