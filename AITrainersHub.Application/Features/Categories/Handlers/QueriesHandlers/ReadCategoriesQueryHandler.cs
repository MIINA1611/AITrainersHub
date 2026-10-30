
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Categories.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadCategoriesQuery to retrieve a list of categories from the database.
    /// </summary>
    public class ReadCategoriesQueryHandler(IAITrainerHubDbContext context) : IRequestHandler<ReadCategoriesQuery, List<CategoryListResponse>>
    {

        public async Task<List<CategoryListResponse>> Handle(ReadCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await context.Categories
                    .Select(c => new CategoryListResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
