using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using FluentValidation;

namespace AITrainersHub.Application.Features.WorkOpportunity.Validators
{
    /// <summary>
    /// Validates the UpdateWorkOpportunityCommand.
    /// </summary>
    public class UpdateWorkOpportunityCommandValidator : AbstractValidator<UpdateWorkOpportunityCommand>
    {
        public UpdateWorkOpportunityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Opportunity Id is required.");

            RuleFor(x => x.PlatformId)
                .Must(platformId => !platformId.HasValue || platformId.Value != Guid.Empty)
                .WithMessage("Platform is invalid.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Opportunity title cannot be empty.")
                .MaximumLength(150).WithMessage("Opportunity title must not exceed 150 characters.")
                .When(x => x.Title is not null);

            RuleFor(x => x.DescriptionAr)
                .NotEmpty().WithMessage("Arabic description cannot be empty.")
                .MaximumLength(2000).WithMessage("Arabic description must not exceed 2000 characters.")
                .When(x => x.DescriptionAr is not null);

            RuleFor(x => x.DescriptionEn)
                .NotEmpty().WithMessage("English description cannot be empty.")
                .MaximumLength(2000).WithMessage("English description must not exceed 2000 characters.")
                .When(x => x.DescriptionEn is not null);

            RuleFor(x => x.WhatsAppLink)
                .NotEmpty().WithMessage("WhatsApp link cannot be empty.")
                .MaximumLength(300).WithMessage("WhatsApp link must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("WhatsApp link is invalid.")
                .When(x => x.WhatsAppLink is not null);
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
