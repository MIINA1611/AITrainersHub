using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformCountry.Validators
{
    /// <summary>
    /// Validates the AddPlatformCountryCommand.
    /// </summary>
    public class AddPlatformCountryCommandValidator : AbstractValidator<AddPlatformCountryCommand>
    {
        public AddPlatformCountryCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.")
                .MustAsync(async (platformId, cancellationToken) =>
                    await context.Platforms.AnyAsync(p => p.Id == platformId, cancellationToken))
                .WithMessage("Platform does not exist.");

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("Country Id is required.")
                .MustAsync(async (countryId, cancellationToken) =>
                    await context.Countries.AnyAsync(c => c.Id == countryId, cancellationToken))
                .WithMessage("Country does not exist.");

            RuleFor(x => x.Notes)
                .MaximumLength(300).WithMessage("Notes must not exceed 300 characters.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    !await context.PlatformCountries.AnyAsync(
                        pc => pc.PlatformId == command.PlatformId && pc.CountryId == command.CountryId,
                        cancellationToken))
                .WithMessage("The country is already linked to this platform.");
        }
    }
}
