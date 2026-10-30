using AITrainersHub.Application.Features.Advertisement.Commands;
using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using AdvertisementEntity = AITrainersHub.Domain.Models.Advertisement;

namespace AITrainersHub.Application.Features.Advertisement.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddAdvertisementCommand and persists a new advertisement.
    /// </summary>
    public class AddAdvertisementCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddAdvertisementCommand, AdvertisementDto>
    {
        public async Task<AdvertisementDto> Handle(
            AddAdvertisementCommand request,
            CancellationToken cancellationToken)
        {
            var advertisement = new AdvertisementEntity(
                request.Title,
                request.Subtitle,
                request.CtaText,
                request.CtaUrl,
                request.Position,
                request.IsActive,
                request.StartDate,
                request.EndDate);

            await context.Advertisements.AddAsync(advertisement, cancellationToken);
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
