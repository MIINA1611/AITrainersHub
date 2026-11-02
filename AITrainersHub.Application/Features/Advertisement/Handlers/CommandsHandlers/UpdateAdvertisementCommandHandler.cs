using System.Net;
using AITrainersHub.Application.Features.Advertisement.Commands;
using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AdvertisementEntity = AITrainersHub.Domain.Models.Advertisement;

namespace AITrainersHub.Application.Features.Advertisement.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateAdvertisementCommand and applies the supplied values.
    /// </summary>
    public class UpdateAdvertisementCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateAdvertisementCommand, AdvertisementDto>
    {
        public async Task<AdvertisementDto> Handle(
            UpdateAdvertisementCommand request,
            CancellationToken cancellationToken)
        {
            var advertisement = await context.Advertisements
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (advertisement == null)
            {
                throw new ResultException($"Advertisement with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            advertisement.Update(
                request.Title,
                request.Subtitle,
                request.CtaText,
                request.CtaUrl,
                request.Position,
                request.IsActive,
                request.StartDate,
                request.EndDate);

            if (request.UpdateSchedule)
            {
                // The entity update keeps the previous value when a date is null.
                // The edit form needs to clear optional dates, so assign directly.
                advertisement.StartDate = request.StartDate;
                advertisement.EndDate = request.EndDate;
            }

            context.Advertisements.Update(advertisement);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadAdvertisementByIdQuery)}_{advertisement.Id}");

            return Map(advertisement);
        }

        private static AdvertisementDto Map(AdvertisementEntity advertisement) => new()
        {
            Id = advertisement.Id,
            Title = advertisement.Title,
            Subtitle = advertisement.Subtitle,
            CtaText = advertisement.CtaText,
            CtaUrl = advertisement.CtaUrl,
            Position = advertisement.Position,
            IsActive = advertisement.IsActive,
            StartDate = advertisement.StartDate,
            EndDate = advertisement.EndDate,
            CreatedAt = advertisement.CreatedAt
        };
    }
}
