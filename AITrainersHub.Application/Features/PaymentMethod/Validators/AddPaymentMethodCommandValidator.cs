using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PaymentMethod.Validators
{
    /// <summary>
    /// Validates the AddPaymentMethodCommand.
    /// </summary>
    public class AddPaymentMethodCommandValidator : AbstractValidator<AddPaymentMethodCommand>
    {
        public AddPaymentMethodCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Payment method name is required.")
                .MaximumLength(100).WithMessage("Payment method name must not exceed 100 characters.")
                .MustAsync(async (name, cancellationToken) =>
                    !await context.PaymentMethods.AnyAsync(pm => pm.Name == name, cancellationToken))
                .WithMessage("Payment method name is already in use.");
        }
    }
}
