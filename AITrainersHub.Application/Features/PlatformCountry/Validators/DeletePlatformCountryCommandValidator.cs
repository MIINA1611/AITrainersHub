using AITrainersHub.Application.Features.PlatformCountry.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.PlatformCountry.Validators
{
    /// <summary>
    /// Validates the DeletePlatformCountryCommand.
    /// </summary>
    public class DeletePlatformCountryCommandValidator : AbstractValidator<DeletePlatformCountryCommand>
    {
        public DeletePlatformCountryCommandValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.");

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("Country Id is required.");
        }
    }
}
