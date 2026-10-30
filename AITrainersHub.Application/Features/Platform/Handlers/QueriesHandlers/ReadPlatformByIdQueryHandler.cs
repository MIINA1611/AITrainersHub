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
    /// Handles the ReadPlatformByIdQuery and returns the full platform details.
    /// </summary>
    public class ReadPlatformByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPlatformByIdQuery, PlatformDetailsDto>
    {
        public async Task<PlatformDetailsDto> Handle(
            ReadPlatformByIdQuery request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(PlatformDetailsProjection.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return platform;
        }
    }
}
