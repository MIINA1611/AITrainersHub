using System.Net;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Platform.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPlatformBySlugQuery and returns the full platform details.
    /// </summary>
    public class ReadPlatformBySlugQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPlatformBySlugQuery, PlatformDetailsDto>
    {
        public async Task<PlatformDetailsDto> Handle(
            ReadPlatformBySlugQuery request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .Where(p => p.Slug == request.Slug)
                .Select(PlatformDetailsProjection.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Slug {request.Slug} not found.", (int)HttpStatusCode.NotFound);
            }

            return platform;
        }
    }
}
