using AITrainersHub.Application.Features.Categories.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Categories.Validators
{
    /// <summary>
    /// Validator for the AddCategoryCommands to ensure that the category data is valid before processing.
    /// </summary>
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommands>
    {
        public AddCategoryCommandValidator()
        {
            // شروط التحقق من اسم الفئة
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required and cannot be empty.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            // شروط التحقق من الوصف (اختياري بس بنحدد أقصى طول ليه)
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
