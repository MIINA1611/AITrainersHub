using System.Net;
using AITrainersHub.Application.Features.Advertisement.Commands;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Advertisement.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteAdvertisementCommand.
    /// </summary>
    public class DeleteAdvertisementCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteAdvertisementCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteAdvertisementCommand request,
            CancellationToken cancellationToken)
        {
            var advertisement = await context.Advertisements
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (advertisement == null)
            {
                throw new ResultException($"Advertisement with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.Advertisements.Remove(advertisement);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadAdvertisementByIdQuery)}_{advertisement.Id}");

            return true;
        }
    }
}
