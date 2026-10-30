using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.Platform.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPlatformCommand and persists a new platform.
    /// </summary>
    public class AddPlatformCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPlatformCommand, PlatformDto>
    {
        public async Task<PlatformDto> Handle(
            AddPlatformCommand request,
            CancellationToken cancellationToken)
        {
            var platform = new PlatformEntity(
                request.Name,
                request.Slug,
                request.DescriptionAr,
                request.DescriptionEn,
                request.WebsiteUrl,
                request.ReferralLink,
                request.Status);

            await context.Platforms.AddAsync(platform, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{platform.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");

            return Map(platform);
        }

        private static PlatformDto Map(PlatformEntity platform) => new()
        {
            Id = platform.Id,
            Name = platform.Name,
            Slug = platform.Slug,
            DescriptionAr = platform.DescriptionAr,
            DescriptionEn = platform.DescriptionEn,
            WebsiteUrl = platform.WebsiteUrl,
            ReferralLink = platform.ReferralLink,
            Status = platform.Status,
            CreatedAt = platform.CreatedAt
        };
    }
}
