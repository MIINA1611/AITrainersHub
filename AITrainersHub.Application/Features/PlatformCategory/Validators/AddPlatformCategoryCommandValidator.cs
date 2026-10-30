using AITrainersHub.Application.Features.PlatformCategory.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.PlatformCategory.Validators
{
    /// <summary>
    /// Validates the AddPlatformCategoryCommand.
    /// </summary>
    public class AddPlatformCategoryCommandValidator : AbstractValidator<AddPlatformCategoryCommand>
    {
        public AddPlatformCategoryCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Platform Id is required.")
                .MustAsync(async (platformId, cancellationToken) =>
                    await context.Platforms.AnyAsync(p => p.Id == platformId, cancellationToken))
                .WithMessage("Platform does not exist.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category Id is required.")
                .MustAsync(async (categoryId, cancellationToken) =>
                    await context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken))
                .WithMessage("Category does not exist.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    !await context.PlatformCategories.AnyAsync(
                        pc => pc.PlatformId == command.PlatformId && pc.CategoryId == command.CategoryId,
                        cancellationToken))
                .WithMessage("The category is already linked to this platform.");
        }
    }
}
