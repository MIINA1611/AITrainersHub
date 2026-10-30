using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Features.WorkOpportunity.Validators
{
    /// <summary>
    /// Validates the AddWorkOpportunityCommand.
    /// </summary>
    public class AddWorkOpportunityCommandValidator : AbstractValidator<AddWorkOpportunityCommand>
    {
        public AddWorkOpportunityCommandValidator(IAITrainerHubDbContext context)
        {
            RuleFor(x => x.LeaderId)
                .NotEmpty().WithMessage("Leader Id is required.")
                .MustAsync(async (leaderId, cancellationToken) =>
                    await context.Users.AnyAsync(u => u.Id == leaderId, cancellationToken))
                .WithMessage("Leader does not exist.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Opportunity title is required.")
                .MaximumLength(150).WithMessage("Opportunity title must not exceed 150 characters.");

            RuleFor(x => x.DescriptionAr)
                .NotEmpty().WithMessage("Arabic description is required.")
                .MaximumLength(2000).WithMessage("Arabic description must not exceed 2000 characters.");

            RuleFor(x => x.DescriptionEn)
                .NotEmpty().WithMessage("English description is required.")
                .MaximumLength(2000).WithMessage("English description must not exceed 2000 characters.");

            RuleFor(x => x.WhatsAppLink)
                .NotEmpty().WithMessage("WhatsApp link is required.")
                .MaximumLength(300).WithMessage("WhatsApp link must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("WhatsApp link is invalid.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
