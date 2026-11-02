using AITrainerHub.Extensions;
using AITrainerHub.Request_Models.Public;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Domain.Models;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize]
    public class SuggestionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IValidator<PlatformSuggestionRequest> _validator;

        public SuggestionsController(
            IMediator mediator,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IValidator<PlatformSuggestionRequest> validator)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var user = await GetActiveUserAsync();
            if (user is null)
            {
                return RedirectToLogin();
            }

            ViewData["Title"] = "Suggest a Platform";

            return View(await BuildViewModelAsync(new PlatformSuggestionRequest
            {
                EmailOfUser = user.Email
            }, cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("SuggestionsPolicy")]
        public async Task<IActionResult> Create(
            [Bind(Prefix = "Form")] PlatformSuggestionRequest form,
            CancellationToken cancellationToken)
        {
            var user = await GetActiveUserAsync();
            if (user is null)
            {
                return RedirectToLogin();
            }

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
                        NormalizeOptional(form.EmailOfUser),
                        form.CategoryId),
                    cancellationToken);

                TempData["Success"] = "Thank you! Your suggestion is pending moderation. Every submission is reviewed before it is published.";
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

            return new SuggestPlatformViewModel
            {
                Form = form,
                Categories = categories.OrderBy(category => category.Name).ToList()
            };
        }

        private async Task<ApplicationUser?> GetActiveUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null || !user.IsActive)
            {
                await _signInManager.SignOutAsync();
                TempData["Error"] = "Please sign in with an active account to suggest a platform.";
                return null;
            }

            return user;
        }

        private IActionResult RedirectToLogin()
        {
            var returnUrl = Url.Action(nameof(Create), "Suggestions", new { area = "Users" })
                ?? Url.Content("~/Users/Suggestions/Create");

            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
