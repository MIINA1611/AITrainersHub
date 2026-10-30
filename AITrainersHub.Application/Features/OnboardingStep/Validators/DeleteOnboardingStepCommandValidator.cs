using AITrainersHub.Application.Features.OnboardingStep.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.OnboardingStep.Validators
{
    /// <summary>
    /// Validates the DeleteOnboardingStepCommand.
    /// </summary>
    public class DeleteOnboardingStepCommandValidator : AbstractValidator<DeleteOnboardingStepCommand>
    {
        public DeleteOnboardingStepCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Step Id is required.");
        }
    }
}
