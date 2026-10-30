using System.Net;
using AITrainersHub.Application.Features.Categories.Commands;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Categories.Handlers.CommandsHandlers
{
    public class UpdateCategoryCommandHandler(IAITrainerHubDbContext context) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (category == null)
            {
                throw new ResultException($"Category with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }
            category.Update(request.Name, request.Description);
            context.Categories.Update(category);


           await context.SaveChangesAsync(cancellationToken);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return categoryDto;
        }
    }
}
