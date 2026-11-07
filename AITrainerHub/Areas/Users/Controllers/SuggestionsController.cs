using AITrainerHub.Extensions;
using AITrainerHub.Request_Models.Public;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    [AllowAnonymous]
    public class SuggestionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<PlatformSuggestionRequest> _validator;

        public SuggestionsController(
            IMediator mediator,
            IValidator<PlatformSuggestionRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Suggest a Platform";

            return View(await BuildViewModelAsync(new PlatformSuggestionRequest(), cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("SuggestionsPolicy")]
        public async Task<IActionResult> Create(
            [Bind(Prefix = "Form")] PlatformSuggestionRequest form,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Suggest a Platform";

            var validation = await _validator.ValidateAsync(form, cancellationToken);
            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState, "Form");
                return View(await BuildViewModelAsync(form, cancellationToken));
            }

            try
            {
                await _mediator.Send(
                    new AddPlatformSuggestionCommand(
                        form.PlatformName.Trim(),
                        form.Url.Trim(),
                        form.Description.Trim(),
                        form.SubmitterName.Trim(),
                        form.CategoryId),
                    cancellationToken);

                TempData["SuggestionSubmitted"] = "1";
                TempData["SuggestionSubmittedName"] = form.SubmitterName.Trim();

                return RedirectToAction(nameof(Create));
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(await BuildViewModelAsync(form, cancellationToken));
            }
            catch (ResultException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(await BuildViewModelAsync(form, cancellationToken));
            }
        }

        private async Task<SuggestPlatformViewModel> BuildViewModelAsync(
            PlatformSuggestionRequest form,
            CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);
            var submitted = TempData["SuggestionSubmitted"] is string;
            var submittedName = TempData["SuggestionSubmittedName"] as string;

            return new SuggestPlatformViewModel
            {
                Form = form,
                Categories = categories.OrderBy(category => category.Name).ToList(),
                Submitted = submitted,
                SubmittedName = submittedName
            };
        }
    }
}
