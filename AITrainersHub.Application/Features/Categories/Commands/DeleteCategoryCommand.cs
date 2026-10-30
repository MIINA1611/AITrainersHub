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
    public class DeleteCategoryCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public IEnumerable<string> CacheKeys => [nameof(ReadCategoriesQuery), $"{nameof(ReadCategoryById)}_{Id}"];

        public DeleteCategoryCommand()
        {
        }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }
}
