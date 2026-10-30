using System.Net;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCategory.Commands;
using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCategory.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformCategoryEntity = AITrainersHub.Domain.Models.PlatformCategory;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.PlatformCategory.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddPlatformCategoryCommand and creates the link.
    /// </summary>
    public class AddPlatformCategoryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddPlatformCategoryCommand, PlatformCategoryDto>
    {
        public async Task<PlatformCategoryDto> Handle(
            AddPlatformCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.PlatformId, cancellationToken);

            if (platform == null)
            {
                throw new ResultException($"Platform with Id {request.PlatformId} not found.", (int)HttpStatusCode.NotFound);
            }

            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                throw new ResultException($"Category with Id {request.CategoryId} not found.", (int)HttpStatusCode.NotFound);
            }

            var platformCategory = new PlatformCategoryEntity(request.PlatformId, request.CategoryId);

            await context.PlatformCategories.AddAsync(platformCategory, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AddCacheKeys(request, platform);

            return new PlatformCategoryDto
            {
                CategoryId = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        private static void AddCacheKeys(AddPlatformCategoryCommand request, PlatformEntity platform)
        {
            request.CacheKeySet.Add($"{nameof(ReadCategoriesByPlatformIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{request.PlatformId}");
            request.CacheKeySet.Add($"{nameof(ReadPlatformBySlugQuery)}_{platform.Slug}");
            request.CacheKeySet.Add(nameof(ReadPlatformsQuery));
        }
    }
}
