using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCountry.Commands
{
    /// <summary>
    /// Represents a command to link a country to a platform.
    /// </summary>
    public class AddPlatformCountryCommand : IRequest<PlatformCountryDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid CountryId { get; set; }
        public bool IsFullySupported { get; set; } = true;
        public string? Notes { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPlatformCountryCommand()
        {
        }

        public AddPlatformCountryCommand(
            Guid platformId,
            Guid countryId,
            bool isFullySupported,
            string? notes)
        {
            PlatformId = platformId;
            CountryId = countryId;
            IsFullySupported = isFullySupported;
            Notes = notes;
        }
    }
}
