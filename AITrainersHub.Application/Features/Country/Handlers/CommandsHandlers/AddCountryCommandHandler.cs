using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using CountryEntity = AITrainersHub.Domain.Models.Country;

namespace AITrainersHub.Application.Features.Country.Handlers.CommandsHandlers
{
    /// <summary>
    /// Handles the AddCountryCommand and persists a new country.
    /// </summary>
    public class AddCountryCommandHandler(IAITrainerHubDbContext context)
        : IRequestHandler<AddCountryCommand, CountryDto>
    {
        public async Task<CountryDto> Handle(
            AddCountryCommand request,
            CancellationToken cancellationToken)
        {
            var country = new CountryEntity(request.NameAr, request.NameEn, request.IsoCode);

            await context.Countries.AddAsync(country, cancellationToken);
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
