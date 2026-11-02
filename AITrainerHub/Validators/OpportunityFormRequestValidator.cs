using AITrainerHub.Request_Models.Leader;
using FluentValidation;

namespace AITrainerHub.Validators
{
    /// <summary>
    /// Validates the Leader opportunity form before it is mapped to a command.
    /// </summary>
    public class OpportunityFormRequestValidator : AbstractValidator<OpportunityFormRequest>
    {
        public OpportunityFormRequestValidator()
        {
            RuleFor(x => x.PlatformId)
                .NotEmpty().WithMessage("Please select a platform.");

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
                .NotEmpty().WithMessage("WhatsApp application link is required.")
                .MaximumLength(300).WithMessage("WhatsApp link must not exceed 300 characters.")
                .Must(BeAValidUrl).WithMessage("WhatsApp link must be a valid http(s) URL.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
