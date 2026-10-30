using System.Net;
using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Country.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the DeleteCountryCommand.
    /// </summary>
    public class DeleteCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<DeleteCountryCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteCountryCommand request,
            CancellationToken cancellationToken)
        {
            var country = await context.Countries
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (country == null)
            {
                throw new ResultException($"Country with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            context.Countries.Remove(country);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadCountryByIdQuery)}_{country.Id}");

            return true;
        }
    }
}
