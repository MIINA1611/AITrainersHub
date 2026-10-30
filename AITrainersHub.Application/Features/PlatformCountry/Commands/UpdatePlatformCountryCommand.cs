using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCountry.Commands
{
    /// <summary>
    /// Represents a command to update the support status and notes of a country linked to a platform.
    /// </summary>
    public class UpdatePlatformCountryCommand : IRequest<PlatformCountryDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid CountryId { get; set; }
        public bool IsFullySupported { get; set; }
        public string? Notes { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdatePlatformCountryCommand()
        {
        }

        public UpdatePlatformCountryCommand(
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
