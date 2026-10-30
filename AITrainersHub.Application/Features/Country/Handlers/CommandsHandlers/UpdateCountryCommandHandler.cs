using System.Net;
using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CountryEntity = AITrainersHub.Domain.Models.Country;

namespace AITrainersHub.Application.Features.Country.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the UpdateCountryCommand and applies the supplied values.
    /// </summary>
    public class UpdateCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<UpdateCountryCommand, CountryDto>
    {
        public async Task<CountryDto> Handle(
            UpdateCountryCommand request,
            CancellationToken cancellationToken)
        {
            var country = await context.Countries
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (country == null)
            {
                throw new ResultException($"Country with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            country.Update(request.NameAr, request.NameEn, request.IsoCode);

            context.Countries.Update(country);
            await context.SaveChangesAsync(cancellationToken);

            request.CacheKeySet.Add($"{nameof(ReadCountryByIdQuery)}_{country.Id}");

            return Map(country);
        }

        private static CountryDto Map(CountryEntity country) => new()
        {
            Id = country.Id,
            NameAr = country.NameAr,
            NameEn = country.NameEn,
            IsoCode = country.IsoCode
        };
    }
}
