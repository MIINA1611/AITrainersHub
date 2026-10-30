using System.Net;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Categories.Handlers.QueriesHandlers
{
    public class ReadCategoryByIdQueryHandler(IAITrainerHubDbContext context) : IRequestHandler<ReadCategoryById, CategoryDto>
    {
        public async Task<CategoryDto> Handle(ReadCategoryById request, CancellationToken cancellationToken)
        {
           var category = await context.Categories
                .Where(c => c.Id == request.Id)
                .Select(x=> new CategoryDto { Id= x.Id, Description= x.Description, Name=x.Name})
                .FirstOrDefaultAsync(cancellationToken);
            if (category == null)
            {
                throw new ResultException($"Category with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }
            return category;
        }
    }
}
