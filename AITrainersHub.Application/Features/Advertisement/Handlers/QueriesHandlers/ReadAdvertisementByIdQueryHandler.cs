using System.Net;
using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Advertisement.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadAdvertisementByIdQuery.
    /// </summary>
    public class ReadAdvertisementByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadAdvertisementByIdQuery, AdvertisementDto>
    {
        public async Task<AdvertisementDto> Handle(
            ReadAdvertisementByIdQuery request,
            CancellationToken cancellationToken)
        {
            var advertisement = await context.Advertisements
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new AdvertisementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Subtitle = a.Subtitle,
                    CtaText = a.CtaText,
                    CtaUrl = a.CtaUrl,
                    Position = a.Position,
                    IsActive = a.IsActive,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (advertisement == null)
            {
                throw new ResultException($"Advertisement with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return advertisement;
        }
    }
}
