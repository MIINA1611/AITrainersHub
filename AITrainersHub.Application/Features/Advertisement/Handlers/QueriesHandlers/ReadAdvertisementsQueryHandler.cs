using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AdvertisementEntity = AITrainersHub.Domain.Models.Advertisement;

namespace AITrainersHub.Application.Features.Advertisement.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadAdvertisementsQuery and applies the optional active-state filter.
    /// </summary>
    public class ReadAdvertisementsQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadAdvertisementsQuery, List<AdvertisementDto>>
    {
        public async Task<List<AdvertisementDto>> Handle(
            ReadAdvertisementsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<AdvertisementEntity> query = context.Advertisements.AsNoTracking();

            if (request.IsActive.HasValue)
            {
                query = query.Where(a => a.IsActive == request.IsActive.Value);
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
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
                .ToListAsync(cancellationToken);
        }
    }
}
