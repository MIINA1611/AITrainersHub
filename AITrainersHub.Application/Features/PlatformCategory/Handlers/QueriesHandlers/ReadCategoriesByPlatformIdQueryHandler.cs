using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCategory.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformCategory.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadCategoriesByPlatformIdQuery.
    /// </summary>
    public class ReadCategoriesByPlatformIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadCategoriesByPlatformIdQuery, List<PlatformCategoryDto>>
    {
        public async Task<List<PlatformCategoryDto>> Handle(
            ReadCategoriesByPlatformIdQuery request,
            CancellationToken cancellationToken)
        {
            return await context.PlatformCategories
                .AsNoTracking()
                .Where(pc => pc.PlatformId == request.PlatformId)
                .OrderBy(pc => pc.Category.Name)
                .Select(pc => new PlatformCategoryDto
                {
                    CategoryId = pc.CategoryId,
                    Name = pc.Category.Name,
                    Description = pc.Category.Description
                })
                .ToListAsync(cancellationToken);
        }
    }
}
