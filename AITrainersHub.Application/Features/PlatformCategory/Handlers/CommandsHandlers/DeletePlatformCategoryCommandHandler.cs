using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCategory.Commands;
using AITrainersHub.Application.Features.PlatformCategory.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformCategory.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeletePlatformCategoryCommand and removes the link.
    /// </summary>
    public class DeletePlatformCategoryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeletePlatformCategoryCommand, bool>
    {
        public async Task<bool> Handle(
            DeletePlatformCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var platformCategory = await context.PlatformCategories
                .FirstOrDefaultAsync(
                    pc => pc.PlatformId == request.PlatformId && pc.CategoryId == request.CategoryId,
                    cancellationToken);

            if (platformCategory == null)
            {
                throw new ResultException("Platform category link not found.", (int)HttpStatusCode.NotFound);
            }

            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            context.PlatformCategories.Remove(platformCategory);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadCategoriesByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));

            if (platform != null)
            {
                request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            }

            return true;
        }
    }
}
