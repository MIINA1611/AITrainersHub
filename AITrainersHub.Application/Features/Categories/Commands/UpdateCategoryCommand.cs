using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string> CacheKeys => [nameof(ReadCategoriesQuery), $"{nameof(ReadCategoryById)}_{Id}"];

        public UpdateCategoryCommand()
        {
        }

        public UpdateCategoryCommand(Guid id, string? name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
