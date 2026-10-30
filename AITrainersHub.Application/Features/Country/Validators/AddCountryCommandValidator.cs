using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Country.Validators
{
    /// <summary>
    /// Validates the AddCountryCommand.
    /// </summary>
    public class AddCountryCommandValidator : AbstractValidator<AddCountryCommand>
    {
        public AddCountryCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name is required.")
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters.");

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("English name is required.")
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters.");

            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(2).WithMessage("Country code must consist of exactly 2 characters.")
                .MustAsync(async (isoCode, cancellationToken) =>
                    !await context.Countries.AnyAsync(c => c.IsoCode == isoCode, cancellationToken))
                .WithMessage("Country code is already in use.");
        }
    }
}
