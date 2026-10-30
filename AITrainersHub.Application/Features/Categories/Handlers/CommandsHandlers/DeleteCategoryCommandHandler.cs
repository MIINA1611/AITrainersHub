using System.Net;
using AITrainersHub.Application.Features.Categories.Commands;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace AITrainersHub.Application.Features.Categories.Handlers.CommandsHandlers
{
    public class DeleteCategoryCommandHandler(IAITrainerHubDbContext context) : IRequestHandler<DeleteCategoryCommand, bool>
    {
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new ResultException($"Category with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
