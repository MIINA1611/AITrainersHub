using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Country.Commands
{
    /// <summary>
    /// Represents a command to create a new country.
    /// </summary>
    public class AddCountryCommand : IRequest<CountryDto>, ICacheInvalidatorCommand
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadCountriesQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddCountryCommand()
        {
        }

        public AddCountryCommand(string nameAr, string nameEn, string isoCode)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            IsoCode = isoCode;
        }
    }
}
