using AITrainersHub.Application.Features.PlatformCategory.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.PlatformCategory.Validators
{
    /// <summary>
    /// Validates the DeletePlatformCategoryCommand.
    /// </summary>
    public class DeletePlatformCategoryCommandValidator : AbstractValidator<DeletePlatformCategoryCommand>
    {
        public DeletePlatformCategoryCommandValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category Id is required.");
        }
    }
}
