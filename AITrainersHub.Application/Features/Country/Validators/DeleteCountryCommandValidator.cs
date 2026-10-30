using AITrainersHub.Application.Features.Country.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Country.Validators
{
    /// <summary>
    /// Validates the DeleteCountryCommand.
    /// </summary>
    public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Country Id is required.");
        }
    }
}
