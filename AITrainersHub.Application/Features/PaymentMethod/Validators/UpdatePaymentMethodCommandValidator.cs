using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PaymentMethod.Validators
{
    /// <summary>
    /// Validates the UpdatePaymentMethodCommand.
    /// </summary>
    public class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommand>
    {
        public UpdatePaymentMethodCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Payment method Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Payment method name cannot be empty.")
                .MaximumLength(100).WithMessage("Payment method name must not exceed 100 characters.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Name)
                .MustAsync(async (command, name, cancellationToken) =>
                    !await context.PaymentMethods.AnyAsync(
                        pm => pm.Name == name && pm.Id != command.Id,
                        cancellationToken))
                .WithMessage("Payment method name is already in use.")
                .When(x => x.Name is not null);
        }
    }
}
