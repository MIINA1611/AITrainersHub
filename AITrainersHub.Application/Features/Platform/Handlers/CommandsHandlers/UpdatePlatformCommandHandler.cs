using System.Net;
using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.Platform.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdatePlatformCommand and applies the supplied values.
    /// </summary>
    public class UpdatePlatformCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdatePlatformCommand, PlatformDto>
    {
        public async Task<PlatformDto> Handle(
            UpdatePlatformCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            var oldSlug = platform.Slug;

            platform.Update(
                request.Name,
                request.Slug,
                request.DescriptionAr,
                request.DescriptionEn,
                request.WebsiteUrl,
                request.ReferralLink,
                request.Status);

            context.Platforms.Update(platform);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{platform.Id}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{oldSlug}");
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
