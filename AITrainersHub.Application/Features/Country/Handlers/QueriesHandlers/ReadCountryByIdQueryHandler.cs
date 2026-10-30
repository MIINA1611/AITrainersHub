using System.Net;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Interfaces;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Country.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadCountryByIdQuery.
    /// </summary>
    public class ReadCountryByIdQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadCountryByIdQuery, CountryDto>
    {
        public async Task<CountryDto> Handle(
            ReadCountryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var country = await context.Countries
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    IsoCode = c.IsoCode
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (country == null)
            {
                throw new ResultException($"Country with Id {request.Id} not found.", (int)HttpStatusCode.NotFound);
            }

            return country;
        }
    }
}
