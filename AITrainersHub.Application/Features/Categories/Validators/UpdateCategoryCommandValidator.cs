using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITrainersHub.Application.Features.Categories.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.Categories.Validators
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category Id is required to perform the update.");

            // يتم الفحص فقط إذا أرسل المستخدم قيمة في حقل الاسم (ليس null)
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name cannot be empty or contain only whitespace.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.")
                .When(x => x.Name is not null);

            // يتم الفحص فقط إذا أرسل المستخدم قيمة في حقل الوصف (ليس null)
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => x.Description is not null);
        }
    }
}
