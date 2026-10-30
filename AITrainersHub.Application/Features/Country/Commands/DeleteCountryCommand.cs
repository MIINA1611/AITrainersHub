using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Country.Commands
{
    /// <summary>
    /// Represents a command to delete a country.
    /// </summary>
    public class DeleteCountryCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadCountriesQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeleteCountryCommand()
        {
        }

        public DeleteCountryCommand(Guid id)
        {
            Id = id;
            CacheKeySet.Add($"{nameof(ReadCountryByIdQuery)}_{id}");
        }
    }
}
