using AITrainersHub.Application.Features.PlatformPaymentMethod.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Validators
{
    /// <summary>
    /// Validates the AddPlatformPaymentMethodCommand.
    /// </summary>
    public class AddPlatformPaymentMethodCommandValidator : AbstractValidator<AddPlatformPaymentMethodCommand>
    {
        public AddPlatformPaymentMethodCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.")
                .MustAsync(async (platformId, cancellationToken) =>
                    await context.Platforms.AnyAsync(p => p.Id == platformId, cancellationToken))
                .WithMessage("Platform does not exist.");

            RuleFor(x => x.PaymentMethodId)
                .NotEmpty().WithMessage("Payment method Id is required.")
                .MustAsync(async (paymentMethodId, cancellationToken) =>
                    await context.PaymentMethods.AnyAsync(pm => pm.Id == paymentMethodId, cancellationToken))
                .WithMessage("Payment method does not exist.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    !await context.PlatformPaymentMethods.AnyAsync(
                        ppm => ppm.PlatformId == command.PlatformId && ppm.PaymentMethodId == command.PaymentMethodId,
                        cancellationToken))
                .WithMessage("The payment method is already linked to this platform.");
        }
    }
}
