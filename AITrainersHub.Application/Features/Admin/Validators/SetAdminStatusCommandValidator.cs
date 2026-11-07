using AITrainersHub.Application.Features.Admin.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Admin.Validators
{
    /// <summary>
    /// Validates the identifiers used when changing an admin account status.
    /// </summary>
    public class SetAdminStatusCommandValidator : AbstractValidator<SetAdminStatusCommand>
    {
        public SetAdminStatusCommandValidator()
        {
            RuleFor(x => x.AdminId)
                .NotEmpty().WithMessage("Admin account is required.");

            RuleFor(x => x.CurrentAdminId)
                .NotEmpty().WithMessage("Current admin account is required.");
        }
    }
}
