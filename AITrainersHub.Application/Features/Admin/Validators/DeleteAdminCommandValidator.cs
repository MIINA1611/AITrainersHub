using AITrainersHub.Application.Features.Admin.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Admin.Validators
{
    /// <summary>
    /// Validates the identifiers used when deleting an admin account.
    /// </summary>
    public class DeleteAdminCommandValidator : AbstractValidator<DeleteAdminCommand>
    {
        public DeleteAdminCommandValidator()
        {
            RuleFor(x => x.AdminId)
                .NotEmpty().WithMessage("Admin account is required.");

            RuleFor(x => x.CurrentAdminId)
                .NotEmpty().WithMessage("Current admin account is required.");
        }
    }
}
