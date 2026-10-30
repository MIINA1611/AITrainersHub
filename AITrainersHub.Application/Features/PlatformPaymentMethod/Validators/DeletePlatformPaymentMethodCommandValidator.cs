using AITrainersHub.Application.Features.PlatformPaymentMethod.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Validators
{
    /// <summary>
    /// Validates the DeletePlatformPaymentMethodCommand.
    /// </summary>
    public class DeletePlatformPaymentMethodCommandValidator : AbstractValidator<DeletePlatformPaymentMethodCommand>
    {
        public DeletePlatformPaymentMethodCommandValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.");

            RuleFor(x => x.PaymentMethodId)
                .NotEmpty().WithMessage("Payment method Id is required.");
        }
    }
}
