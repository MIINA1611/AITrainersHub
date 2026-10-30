using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformCountry.Validators
{
    /// <summary>
    /// Validates the UpdatePlatformCountryCommand.
    /// </summary>
    public class UpdatePlatformCountryCommandValidator : AbstractValidator<UpdatePlatformCountryCommand>
    {
        public UpdatePlatformCountryCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.");

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("Country Id is required.");

            RuleFor(x => x.Notes)
                .MaximumLength(300).WithMessage("Notes must not exceed 300 characters.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    await context.PlatformCountries.AnyAsync(
                        pc => pc.PlatformId == command.PlatformId && pc.CountryId == command.CountryId,
                        cancellationToken))
                .WithMessage("The country is not linked to this platform.");
        }
    }
}
