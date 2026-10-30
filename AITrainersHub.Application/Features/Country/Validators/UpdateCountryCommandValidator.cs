using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.Country.Validators
{
    /// <summary>
    /// Validates the UpdateCountryCommand.
    /// </summary>
    public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Country Id is required.");

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name cannot be empty.")
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters.")
                .When(x => x.NameAr is not null);

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("English name cannot be empty.")
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters.")
                .When(x => x.NameEn is not null);

            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("Country code cannot be empty.")
                .Length(2).WithMessage("Country code must consist of exactly 2 characters.")
                .When(x => x.IsoCode is not null);

            RuleFor(x => x.IsoCode)
                .MustAsync(async (command, isoCode, cancellationToken) =>
                    !await context.Countries.AnyAsync(
                        c => c.IsoCode == isoCode && c.Id != command.Id,
                        cancellationToken))
                .WithMessage("Country code is already in use.")
                .When(x => x.IsoCode is not null);
        }
    }
}
