using AITrainerHub.Extensions;
using AITrainerHub.Presentation;
using AITrainerHub.Request_Models.Public;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    public class PlatformsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<PublicReviewRequest> _reviewValidator;

        public PlatformsController(
            IMediator mediator,
            IValidator<PublicReviewRequest> reviewValidator)
        {
            _mediator = mediator;
            _reviewValidator = reviewValidator;
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

            var directory = PlatformDirectoryBuilder.Build(
                platforms,
                categories,
                countries,
                paymentMethods,
                search,
                status,
                category,
                country,
                payment,
                page);

            ViewData["Title"] = "Explore AI Training Platforms";

            return View(new PublicPlatformsIndexViewModel
            {
                Platforms = directory.Platforms,
                Categories = directory.Categories,
                Countries = directory.Countries,
                PaymentMethods = directory.PaymentMethods,
                Search = directory.Search,
                Status = directory.Status,
                CategoryId = directory.CategoryId,
                CountryId = directory.CountryId,
                PaymentMethodId = directory.PaymentMethodId,
                Page = directory.Page,
                PageSize = directory.PageSize,
                TotalCount = directory.TotalCount,
                TotalPages = directory.TotalPages
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

            ViewData["Title"] = $"{platform.Name} — Platform Details";

            return View(new PlatformDetailsViewModel
            {
                Platform = platform,
                RelatedOpportunities = opportunities
                    .Where(opportunity => opportunity.PlatformId == platform.Id)
                    .Take(6)
                    .ToList(),
                ReviewForm = new PublicReviewRequest
                {
                    PlatformId = platform.Id,
                    Rating = 5
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("ReviewsPolicy")]
        public async Task<IActionResult> SubmitReview(
            PublicReviewRequest form,
            CancellationToken cancellationToken)
        {
            var redirectUrl = BuildDetailsUrl(form.PlatformId, "reviews");

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
                        ReviewerName = form.ReviewerName.Trim(),
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

        private string BuildDetailsUrl(Guid platformId, string section)
        {
            var detailsUrl = Url.Action(nameof(Details), new { id = platformId.ToString() })
                ?? Url.Content("~/Users/Platforms");
            return $"{detailsUrl}#{section}";
        }
    }
}

