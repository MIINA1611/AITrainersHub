using AITrainersHub.Application.Features.Categories.Commands;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using MediatR;


namespace AITrainersHub.Application.Features.Categories.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddCategoryCommands to add a new category to the database.
    /// </summary>
    public class AddCategoryCommandHandler(IAITrainerHubDbContext context) : IRequestHandler<AddCategoryCommands, CategoryDto>
    {

        public async Task<CategoryDto> Handle(AddCategoryCommands request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Name, request.Description);

            await context.Categories.AddAsync(category, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var CategoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return CategoryDto;

        }
    }
}
 