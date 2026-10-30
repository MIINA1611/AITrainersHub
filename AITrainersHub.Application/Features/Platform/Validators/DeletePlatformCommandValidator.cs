using AITrainersHub.Application.Features.Platform.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Platform.Validators
{
    /// <summary>
    /// Validates the DeletePlatformCommand before the platform is deleted.
    /// </summary>
    public class DeletePlatformCommandValidator : AbstractValidator<DeletePlatformCommand>
    {
        public DeletePlatformCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Platform Id is required.");
        }
    }
}
