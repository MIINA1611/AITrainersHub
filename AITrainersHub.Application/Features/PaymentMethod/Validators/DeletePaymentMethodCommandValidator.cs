using AITrainersHub.Application.Features.PaymentMethod.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.PaymentMethod.Validators
{
    /// <summary>
    /// Validates the DeletePaymentMethodCommand.
    /// </summary>
    public class DeletePaymentMethodCommandValidator : AbstractValidator<DeletePaymentMethodCommand>
    {
        public DeletePaymentMethodCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Payment method Id is required.");
        }
    }
}
