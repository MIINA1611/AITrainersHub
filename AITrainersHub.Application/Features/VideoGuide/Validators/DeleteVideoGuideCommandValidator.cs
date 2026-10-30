using AITrainersHub.Application.Features.VideoGuide.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.VideoGuide.Validators
{
    /// <summary>
    /// Validates the DeleteVideoGuideCommand.
    /// </summary>
    public class DeleteVideoGuideCommandValidator : AbstractValidator<DeleteVideoGuideCommand>
    {
        public DeleteVideoGuideCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Guide Id is required.");
        }
    }
}
