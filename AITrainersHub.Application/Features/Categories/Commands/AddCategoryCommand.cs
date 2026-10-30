using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
namespace AITrainersHub.Application.Features.Categories.Commands
{
    public class AddCategoryCommands : IRequest<CategoryDto> , ICacheInvalidatorCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IEnumerable<string> CacheKeys => [nameof(ReadCategoriesQuery)];

        public AddCategoryCommands()
        {
        }

        public AddCategoryCommands(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}
