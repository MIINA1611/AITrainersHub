using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Country.Commands
{
    /// <summary>
    /// Represents a command to update an existing country.
    /// </summary>
    public class UpdateCountryCommand : IRequest<CountryDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? IsoCode { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadCountriesQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateCountryCommand()
        {
        }

        public UpdateCountryCommand(Guid id, string? nameAr, string? nameEn, string? isoCode)
        {
            Id = id;
            NameAr = nameAr;
            NameEn = nameEn;
            IsoCode = isoCode;
            CacheKeySet.Add($"{nameof(ReadCountryByIdQuery)}_{id}");
        }
    }
}
