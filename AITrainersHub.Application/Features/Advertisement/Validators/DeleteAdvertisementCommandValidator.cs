using AITrainersHub.Application.Features.Advertisement.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Advertisement.Validators
{
    /// <summary>
    /// Validates the DeleteAdvertisementCommand.
    /// </summary>
    public class DeleteAdvertisementCommandValidator : AbstractValidator<DeleteAdvertisementCommand>
    {
        public DeleteAdvertisementCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Advertisement Id is required.");
        }
    }
}
