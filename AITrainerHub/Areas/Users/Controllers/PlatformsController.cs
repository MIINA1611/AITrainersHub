using AITrainerHub.Extensions;
using AITrainerHub.Request_Models.Public;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Models;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    public class PlatformsController : Controller
    {
        private const int PageSize = 9;

        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IValidator<PublicReviewRequest> _reviewValidator;
        private readonly IValidator<PublicVideoGuideRequest> _videoGuideValidator;

        public PlatformsController(
            IMediator mediator,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IValidator<PublicReviewRequest> reviewValidator,
            IValidator<PublicVideoGuideRequest> videoGuideValidator)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _reviewValidator = reviewValidator;
            _videoGuideValidator = videoGuideValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            Guid? category,
            Guid? country,
            Guid? payment,
            int page = 1,
            CancellationToken cancellationToken = default)
        {
            // Use the unfiltered query so every platform command invalidates the
            // same cache key; filtering happens in memory for the public directory.
            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);
            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);
            var countries = await _mediator.Send(new ReadCountriesQuery(), cancellationToken);
            var paymentMethods = await _mediator.Send(new ReadPaymentMethodsQuery(), cancellationToken);

            PlatformStatus? statusFilter = null;
            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<PlatformStatus>(status, true, out var parsedStatus))
            {
                statusFilter = parsedStatus;
            }

            IEnumerable<PlatformListResponse> filtered = platforms;

            if (statusFilter.HasValue)
            {
                filtered = filtered.Where(platform => platform.Status == statusFilter.Value);
            }

            if (category.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.Categories.Any(item => item.CategoryId == category.Value));
            }

            if (country.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.Countries.Any(item => item.CountryId == country.Value));
            }

            if (payment.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.PaymentMethods.Any(item => item.PaymentMethodId == payment.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                filtered = filtered.Where(platform =>
                    platform.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    platform.Slug.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            var filteredList = filtered
                .OrderBy(platform => platform.Name)
                .ToList();

            if (page < 1)
            {
                page = 1;
            }

            var totalPages = filteredList.Count == 0
                ? 1
                : (int)Math.Ceiling(filteredList.Count / (double)PageSize);

            if (page > totalPages)
            {
                page = totalPages;
            }

            ViewData["Title"] = "Platforms";

            return View(new PublicPlatformsIndexViewModel
            {
                Platforms = filteredList
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                Categories = categories,
                Countries = countries,
                PaymentMethods = paymentMethods,
                Search = search,
                Status = statusFilter,
                CategoryId = category,
                CountryId = country,
                PaymentMethodId = payment,
                Page = page,
                PageSize = PageSize,
                TotalCount = filteredList.Count,
                TotalPages = totalPages
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Error"] = "Platform not found.";
                return RedirectToAction(nameof(Index));
            }

            PlatformDetailsDto platform;

            try
            {
                platform = Guid.TryParse(id, out var platformId)
                    ? await _mediator.Send(new ReadPlatformByIdQuery(platformId), cancellationToken)
                    : await _mediator.Send(new ReadPlatformBySlugQuery(id), cancellationToken);
            }
            catch (ResultException)
            {
                TempData["Error"] = "Platform not found.";
                return RedirectToAction(nameof(Index));
            }

            var opportunities = await _mediator.Send(
                new ReadWorkOpportunitiesQuery(null, true, true),
                cancellationToken);

            var user = User.Identity?.IsAuthenticated == true
                ? await _userManager.GetUserAsync(User)
                : null;

            var isActiveUser = user is { IsActive: true };
            var displayName = user?.DisplayName
                ?? user?.UserName
                ?? user?.Email
                ?? string.Empty;

            ViewData["Title"] = platform.Name;

            return View(new PlatformDetailsViewModel
            {
                Platform = platform,
                RelatedOpportunities = opportunities
                    .Where(opportunity => opportunity.PlatformId == platform.Id)
                    .Take(6)
                    .ToList(),
                IsAuthenticated = isActiveUser,
                CurrentUserDisplayName = displayName,
                ReviewForm = new PublicReviewRequest
                {
                    PlatformId = platform.Id,
                    ReviewerName = displayName,
                    Rating = 5
                },
                VideoGuideForm = new PublicVideoGuideRequest
                {
                    PlatformId = platform.Id,
                    SourceType = "YouTube",
                    Language = AITrainersHub.Domain.Enums.GuideLanguage.Arabic
                }
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("ReviewsPolicy")]
        public async Task<IActionResult> SubmitReview(
            PublicReviewRequest form,
            CancellationToken cancellationToken)
        {
            var redirectUrl = BuildDetailsUrl(form.PlatformId, "reviews");

            var user = await _userManager.GetUserAsync(User);
            if (user is null || !user.IsActive)
            {
                await _signInManager.SignOutAsync();
                TempData["Error"] = "Please sign in with an active account to submit a review.";
                return RedirectToLogin(form.PlatformId);
            }

            var validation = await _reviewValidator.ValidateAsync(form, cancellationToken);
            if (!validation.IsValid)
            {
                TempData["Error"] = string.Join(
                    " ",
                    validation.Errors
                        .Select(error => error.ErrorMessage)
                        .Where(message => !string.IsNullOrWhiteSpace(message))
                        .Distinct());
                return Redirect(redirectUrl);
            }

            try
            {
                await _mediator.Send(
                    new AddReviewCommand
                    {
                        PlatformId = form.PlatformId,
                        ReviewerName = string.IsNullOrWhiteSpace(form.ReviewerName)
                            ? user.DisplayName ?? user.UserName ?? "AI Trainer"
                            : form.ReviewerName.Trim(),
                        Rating = form.Rating,
                        Comment = form.Comment.Trim()
                    },
                    cancellationToken);

                TempData["Success"] = "Thank you! Your review was submitted and is pending moderation.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return Redirect(redirectUrl);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("VideoGuidesPolicy")]
        public async Task<IActionResult> SubmitVideoGuide(
            PublicVideoGuideRequest form,
            CancellationToken cancellationToken)
        {
            var redirectUrl = BuildDetailsUrl(form.PlatformId, "guides");

            var user = await _userManager.GetUserAsync(User);
            if (user is null || !user.IsActive)
            {
                await _signInManager.SignOutAsync();
                TempData["Error"] = "Please sign in with an active account to submit a video guide.";
                return RedirectToLogin(form.PlatformId);
            }

            var validation = await _videoGuideValidator.ValidateAsync(form, cancellationToken);
            if (!validation.IsValid)
            {
                TempData["Error"] = string.Join(
                    " ",
                    validation.Errors
                        .Select(error => error.ErrorMessage)
                        .Where(message => !string.IsNullOrWhiteSpace(message))
                        .Distinct());
                return Redirect(redirectUrl);
            }

            try
            {
                await _mediator.Send(
                    new AddVideoGuideCommand
                    {
                        PlatformId = form.PlatformId,
                        Title = form.Title.Trim(),
                        Language = form.Language,
                        Url = form.Url.Trim(),
                        SourceType = string.IsNullOrWhiteSpace(form.SourceType)
                            ? "YouTube"
                            : form.SourceType.Trim(),
                        AuthorOrChannel = NormalizeOptional(form.AuthorOrChannel),
                        PublishedAt = form.PublishedAt
                    },
                    cancellationToken);

                TempData["Success"] = "Thank you! Your video guide was submitted and is pending moderation.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return Redirect(redirectUrl);
        }

        private string BuildDetailsUrl(Guid platformId, string section)
        {
            var detailsUrl = Url.Action(nameof(Details), new { id = platformId.ToString() })
                ?? Url.Content("~/Users/Platforms");
            return $"{detailsUrl}#{section}";
        }

        private IActionResult RedirectToLogin(Guid platformId)
        {
            var returnUrl = Url.Action(nameof(Details), new { id = platformId.ToString() })
                ?? Url.Content("~/Users/Platforms");

            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}

