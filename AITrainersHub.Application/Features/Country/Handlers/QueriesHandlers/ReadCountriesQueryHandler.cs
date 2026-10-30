using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Country.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadCountriesQuery.
    /// </summary>
    public class ReadCountriesQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadCountriesQuery, List<CountryDto>>
    {
        public async Task<List<CountryDto>> Handle(
            ReadCountriesQuery request,
            CancellationToken cancellationToken)
        {
            return await context.Countries
                .AsNoTracking()
                .OrderBy(c => c.NameEn)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    IsoCode = c.IsoCode
                })
                .ToListAsync(cancellationToken);
        }
    }
}
