using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCountry.Commands
{
    /// <summary>
    /// Represents a command to unlink a country from a platform.
    /// </summary>
    public class DeletePlatformCountryCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid CountryId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePlatformCountryCommand()
        {
        }

        public DeletePlatformCountryCommand(Guid platformId, Guid countryId)
        {
            PlatformId = platformId;
            CountryId = countryId;
        }
    }
}
