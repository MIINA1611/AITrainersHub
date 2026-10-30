using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.WorkOpportunity.Validators
{
    /// <summary>
    /// Validates the DeleteWorkOpportunityCommand.
    /// </summary>
    public class DeleteWorkOpportunityCommandValidator : AbstractValidator<DeleteWorkOpportunityCommand>
    {
        public DeleteWorkOpportunityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Opportunity Id is required.");
        }
    }
}
