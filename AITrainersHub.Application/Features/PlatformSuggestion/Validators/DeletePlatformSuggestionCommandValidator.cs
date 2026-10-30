using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.PlatformSuggestion.Validators
{
    /// <summary>
    /// Validates the DeletePlatformSuggestionCommand.
    /// </summary>
    public class DeletePlatformSuggestionCommandValidator : AbstractValidator<DeletePlatformSuggestionCommand>
    {
        public DeletePlatformSuggestionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Suggestion Id is required.");
        }
    }
}
