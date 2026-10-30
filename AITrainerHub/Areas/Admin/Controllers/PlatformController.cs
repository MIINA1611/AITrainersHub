using AITrainerHub.Extensions;
using AITrainerHub.Request_Models;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Features.OnboardingStep.Commands;
using AITrainersHub.Application.Features.OnboardingStep.Queries;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using AITrainersHub.Application.Features.Platform.Commands;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformCategory.Commands;
using AITrainersHub.Application.Features.PlatformCategory.Queries;
using AITrainersHub.Application.Features.PlatformCountry.Commands;
using AITrainersHub.Application.Features.PlatformCountry.Queries;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Commands;
using AITrainersHub.Application.Features.PlatformPaymentMethod.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Area.Admin.Controllers
{
    [Area("Admin")]
    public class PlatformController : Controller
    {
        private const int PageSize = 10;

        private readonly IMediator _mediator;

        public PlatformController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            int page = 1,
            CancellationToken cancellationToken = default)
        {
            // The unfiltered query is used on purpose: platform commands invalidate
            // this cache key, while the status-filtered cache keys are not invalidated.
            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);

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

            var rows = new List<PlatformRowViewModel>();

            foreach (var platform in filteredList.Skip((page - 1) * PageSize).Take(PageSize))
            {
                var categories = await _mediator.Send(
                    new ReadCategoriesByPlatformIdQuery(platform.Id),
                    cancellationToken);

                rows.Add(new PlatformRowViewModel
                {
                    Id = platform.Id,
                    Name = platform.Name,
                    Slug = platform.Slug,
                    Status = platform.Status,
                    Categories = string.Join(", ", categories.Select(category => category.Name)),
                    CreatedAt = platform.CreatedAt
                });
            }

            ViewData["Title"] = "Platforms";

            return View(new PlatformsIndexViewModel
            {
                Platforms = rows,
                Search = search,
                Status = statusFilter,
                Page = page,
                PageSize = PageSize,
                TotalCount = filteredList.Count
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Add Platform";
            return View(new PlatformFormRequest { Status = PlatformStatus.Open });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlatformFormRequest request, CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Add Platform";

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var created = await _mediator.Send(
                    new AddPlatformCommand
                    {
                        Name = request.Name.Trim(),
                        Slug = request.Slug.Trim(),
                        DescriptionAr = request.DescriptionAr.Trim(),
                        DescriptionEn = request.DescriptionEn.Trim(),
                        WebsiteUrl = request.WebsiteUrl.Trim(),
                        ReferralLink = NormalizeOptional(request.ReferralLink),
                        Status = request.Status
                    },
                    cancellationToken);

                TempData["Success"] = "Platform created successfully. You can now add its categories, countries and other details.";
                return RedirectToAction(nameof(Edit), new { id = created.Id });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(request);
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var model = await BuildEditViewModelAsync(id, null, cancellationToken);

            if (model is null)
            {
                TempData["Error"] = "Platform not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Edit Platform";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind(Prefix = "Form")] PlatformFormRequest form,
            CancellationToken cancellationToken)
        {
            if (id != form.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var invalidModel = await BuildEditViewModelAsync(id, form, cancellationToken);

                if (invalidModel is null)
                {
                    return NotFound();
                }

                ViewData["Title"] = "Edit Platform";
                return View(invalidModel);
            }

            try
            {
                await _mediator.Send(
                    new UpdatePlatformCommand
                    {
                        Id = id,
                        Name = form.Name.Trim(),
                        Slug = form.Slug.Trim(),
                        DescriptionAr = form.DescriptionAr.Trim(),
                        DescriptionEn = form.DescriptionEn.Trim(),
                        WebsiteUrl = form.WebsiteUrl.Trim(),
                        // Empty string clears the optional value because the entity
                        // update method only keeps the previous value for null.
                        ReferralLink = NormalizeOptional(form.ReferralLink) ?? string.Empty,
                        Status = form.Status
                    },
                    cancellationToken);

                TempData["Success"] = "Platform updated successfully.";
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);

                var invalidModel = await BuildEditViewModelAsync(id, form, cancellationToken);

                if (invalidModel is null)
                {
                    return NotFound();
                }

                ViewData["Title"] = "Edit Platform";
                return View(invalidModel);
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return RedirectToAction(nameof(Edit), new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeletePlatformCommand(id), cancellationToken);
                TempData["Success"] = "Platform deleted successfully.";
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            Guid id,
            PlatformStatus status,
            string? returnUrl,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || !Enum.IsDefined(typeof(PlatformStatus), status))
            {
                TempData["Error"] = "Please choose a valid platform status.";
                return RedirectBack(returnUrl);
            }

            try
            {
                await _mediator.Send(
                    new UpdatePlatformCommand { Id = id, Status = status },
                    cancellationToken);

                TempData["Success"] = "Platform status updated successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectBack(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(
            PlatformCategoryLinkRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }

            return await RunPlatformActionAsync(
                request.PlatformId,
                async () => await _mediator.Send(
                    new AddPlatformCategoryCommand
                    {
                        PlatformId = request.PlatformId,
                        CategoryId = request.CategoryId
                    },
                    cancellationToken),
                "Category linked successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RemoveCategory(
            Guid platformId,
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(
                    new DeletePlatformCategoryCommand(platformId, categoryId),
                    cancellationToken),
                "Category unlinked successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountry(
            PlatformCountryLinkRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }

            return await RunPlatformActionAsync(
                request.PlatformId,
                async () => await _mediator.Send(
                    new AddPlatformCountryCommand
                    {
                        PlatformId = request.PlatformId,
                        CountryId = request.CountryId,
                        IsFullySupported = request.IsFullySupported,
                        Notes = NormalizeOptional(request.Notes)
                    },
                    cancellationToken),
                "Country linked successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(
            PlatformCountryLinkRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }

            return await RunPlatformActionAsync(
                request.PlatformId,
                async () => await _mediator.Send(
                    new UpdatePlatformCountryCommand
                    {
                        PlatformId = request.PlatformId,
                        CountryId = request.CountryId,
                        IsFullySupported = request.IsFullySupported,
                        Notes = NormalizeOptional(request.Notes) ?? string.Empty
                    },
                    cancellationToken),
                "Country settings updated successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RemoveCountry(
            Guid platformId,
            Guid countryId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(
                    new DeletePlatformCountryCommand(platformId, countryId),
                    cancellationToken),
                "Country unlinked successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymentMethod(
            PlatformPaymentMethodLinkRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }

            return await RunPlatformActionAsync(
                request.PlatformId,
                async () => await _mediator.Send(
                    new AddPlatformPaymentMethodCommand
                    {
                        PlatformId = request.PlatformId,
                        PaymentMethodId = request.PaymentMethodId
                    },
                    cancellationToken),
                "Payment method linked successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RemovePaymentMethod(
            Guid platformId,
            Guid paymentMethodId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(
                    new DeletePlatformPaymentMethodCommand(platformId, paymentMethodId),
                    cancellationToken),
                "Payment method unlinked successfully.",
                cancellationToken);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOnboardingStep(Guid platformId, CancellationToken cancellationToken)
        {
            var platform = await _mediator.Send(new ReadPlatformByIdQuery(platformId), cancellationToken);
            var nextStep = platform.OnboardingSteps.Count == 0
                ? 1
                : platform.OnboardingSteps.Max(step => step.StepNumber) + 1;

            await PopulateOnboardingStepViewDataAsync(platformId, "Add Onboarding Step", cancellationToken);

            return View("OnboardingStepForm", new OnboardingStepFormRequest
            {
                PlatformId = platformId,
                StepNumber = nextStep
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditOnboardingStep(Guid id, CancellationToken cancellationToken)
        {
            var step = await _mediator.Send(new ReadOnboardingStepByIdQuery(id), cancellationToken);

            await PopulateOnboardingStepViewDataAsync(step.PlatformId, "Edit Onboarding Step", cancellationToken);

            return View("OnboardingStepForm", new OnboardingStepFormRequest
            {
                Id = step.Id,
                PlatformId = step.PlatformId,
                StepNumber = step.StepNumber,
                TitleAr = step.TitleAr,
                TitleEn = step.TitleEn,
                DescriptionAr = step.DescriptionAr,
                DescriptionEn = step.DescriptionEn,
                Tip = step.Tip,
                ScreenshotUrl = step.ScreenshotUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOnboardingStep(
            OnboardingStepFormRequest request,
            CancellationToken cancellationToken)
        {
            var isNew = request.Id == Guid.Empty;
            var title = isNew ? "Add Onboarding Step" : "Edit Onboarding Step";

            if (!ModelState.IsValid)
            {
                await PopulateOnboardingStepViewDataAsync(request.PlatformId, title, cancellationToken);
                return View("OnboardingStepForm", request);
            }

            try
            {
                if (isNew)
                {
                    await _mediator.Send(
                        new AddOnboardingStepCommand
                        {
                            PlatformId = request.PlatformId,
                            StepNumber = request.StepNumber,
                            TitleAr = request.TitleAr.Trim(),
                            TitleEn = request.TitleEn.Trim(),
                            DescriptionAr = request.DescriptionAr.Trim(),
                            DescriptionEn = request.DescriptionEn.Trim(),
                            Tip = NormalizeOptional(request.Tip),
                            ScreenshotUrl = NormalizeOptional(request.ScreenshotUrl)
                        },
                        cancellationToken);
                }
                else
                {
                    await _mediator.Send(
                        new UpdateOnboardingStepCommand
                        {
                            Id = request.Id,
                            StepNumber = request.StepNumber,
                            TitleAr = request.TitleAr.Trim(),
                            TitleEn = request.TitleEn.Trim(),
                            DescriptionAr = request.DescriptionAr.Trim(),
                            DescriptionEn = request.DescriptionEn.Trim(),
                            Tip = NormalizeOptional(request.Tip),
                            ScreenshotUrl = NormalizeOptional(request.ScreenshotUrl)
                        },
                        cancellationToken);
                }

                TempData["Success"] = isNew
                    ? "Onboarding step added successfully."
                    : "Onboarding step updated successfully.";

                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                await PopulateOnboardingStepViewDataAsync(request.PlatformId, title, cancellationToken);
                return View("OnboardingStepForm", request);
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteOnboardingStep(
            Guid id,
            Guid platformId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(new DeleteOnboardingStepCommand(id), cancellationToken),
                "Onboarding step deleted successfully.",
                cancellationToken);
        }

        [HttpGet]
        public async Task<IActionResult> CreateVideoGuide(Guid platformId, CancellationToken cancellationToken)
        {
            await PopulateVideoGuideViewDataAsync(platformId, "Add Video Guide", cancellationToken);

            return View("VideoGuideForm", new VideoGuideFormRequest
            {
                PlatformId = platformId,
                SourceType = "YouTube"
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditVideoGuide(Guid id, CancellationToken cancellationToken)
        {
            var guide = await _mediator.Send(new ReadVideoGuideByIdQuery(id), cancellationToken);

            await PopulateVideoGuideViewDataAsync(guide.PlatformId, "Edit Video Guide", cancellationToken);

            return View("VideoGuideForm", new VideoGuideFormRequest
            {
                Id = guide.Id,
                PlatformId = guide.PlatformId,
                Title = guide.Title,
                Language = guide.Language,
                Url = guide.Url,
                SourceType = guide.SourceType,
                AuthorOrChannel = guide.AuthorOrChannel,
                PublishedAt = guide.PublishedAt
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveVideoGuide(
            VideoGuideFormRequest request,
            CancellationToken cancellationToken)
        {
            var isNew = request.Id == Guid.Empty;
            var title = isNew ? "Add Video Guide" : "Edit Video Guide";

            if (!ModelState.IsValid)
            {
                await PopulateVideoGuideViewDataAsync(request.PlatformId, title, cancellationToken);
                return View("VideoGuideForm", request);
            }

            try
            {
                if (isNew)
                {
                    await _mediator.Send(
                        new AddVideoGuideCommand
                        {
                            PlatformId = request.PlatformId,
                            Title = request.Title.Trim(),
                            Language = request.Language,
                            Url = request.Url.Trim(),
                            SourceType = request.SourceType.Trim(),
                            AuthorOrChannel = NormalizeOptional(request.AuthorOrChannel),
                            PublishedAt = request.PublishedAt
                        },
                        cancellationToken);
                }
                else
                {
                    await _mediator.Send(
                        new UpdateVideoGuideCommand
                        {
                            Id = request.Id,
                            Title = request.Title.Trim(),
                            Language = request.Language,
                            Url = request.Url.Trim(),
                            SourceType = request.SourceType.Trim(),
                            AuthorOrChannel = NormalizeOptional(request.AuthorOrChannel),
                            PublishedAt = request.PublishedAt
                        },
                        cancellationToken);
                }

                TempData["Success"] = isNew
                    ? "Video guide added successfully."
                    : "Video guide updated successfully.";

                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                await PopulateVideoGuideViewDataAsync(request.PlatformId, title, cancellationToken);
                return View("VideoGuideForm", request);
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteVideoGuide(
            Guid id,
            Guid platformId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(new DeleteVideoGuideCommand(id), cancellationToken),
                "Video guide deleted successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(
            ReviewFormRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Edit), new { id = request.PlatformId });
            }

            return await RunPlatformActionAsync(
                request.PlatformId,
                async () => await _mediator.Send(
                    new AddReviewCommand
                    {
                        PlatformId = request.PlatformId,
                        ReviewerName = request.ReviewerName.Trim(),
                        Rating = request.Rating,
                        Comment = request.Comment.Trim()
                    },
                    cancellationToken),
                "Review added successfully.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SetReviewApproval(
            Guid id,
            Guid platformId,
            bool isApproved,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(
                    new UpdateReviewCommand { Id = id, IsApproved = isApproved },
                    cancellationToken),
                isApproved ? "Review approved successfully." : "Review marked as pending.",
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteReview(
            Guid id,
            Guid platformId,
            CancellationToken cancellationToken)
        {
            return RunPlatformActionAsync(
                platformId,
                async () => await _mediator.Send(new DeleteReviewCommand(id), cancellationToken),
                "Review deleted successfully.",
                cancellationToken);
        }

        private async Task<PlatformEditViewModel?> BuildEditViewModelAsync(
            Guid id,
            PlatformFormRequest? form,
            CancellationToken cancellationToken)
        {
            PlatformDetailsDto details;

            try
            {
                details = await _mediator.Send(new ReadPlatformByIdQuery(id), cancellationToken);
            }
            catch (ResultException)
            {
                return null;
            }

            var reviews = await _mediator.Send(new ReadReviewsByPlatformIdQuery(id), cancellationToken);
            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);
            var countries = await _mediator.Send(new ReadCountriesQuery(), cancellationToken);
            var paymentMethods = await _mediator.Send(new ReadPaymentMethodsQuery(), cancellationToken);

            if (form is not null)
            {
                form.Id = id;
            }

            return new PlatformEditViewModel
            {
                Form = form ?? new PlatformFormRequest
                {
                    Id = details.Id,
                    Name = details.Name,
                    Slug = details.Slug,
                    DescriptionAr = details.DescriptionAr,
                    DescriptionEn = details.DescriptionEn,
                    WebsiteUrl = details.WebsiteUrl,
                    ReferralLink = details.ReferralLink,
                    Status = details.Status
                },
                Details = details,
                Reviews = reviews,
                AllCategories = categories,
                AllCountries = countries,
                AllPaymentMethods = paymentMethods
            };
        }

        private async Task PopulateOnboardingStepViewDataAsync(
            Guid platformId,
            string title,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = title;
            ViewData["PlatformId"] = platformId;
            ViewData["PlatformName"] = await GetPlatformNameAsync(platformId, cancellationToken);
        }

        private async Task PopulateVideoGuideViewDataAsync(
            Guid platformId,
            string title,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = title;
            ViewData["PlatformId"] = platformId;
            ViewData["PlatformName"] = await GetPlatformNameAsync(platformId, cancellationToken);
        }

        private async Task<string> GetPlatformNameAsync(Guid platformId, CancellationToken cancellationToken)
        {
            try
            {
                var platform = await _mediator.Send(new ReadPlatformByIdQuery(platformId), cancellationToken);
                return platform.Name;
            }
            catch (ResultException)
            {
                return "Platform";
            }
        }

        private async Task<IActionResult> RunPlatformActionAsync(
            Guid platformId,
            Func<Task> action,
            string successMessage,
            CancellationToken cancellationToken)
        {
            try
            {
                await action();
                TempData["Success"] = successMessage;
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Edit), new { id = platformId });
        }

        private void SetModelStateError()
        {
            var messages = ModelState.Values
                .SelectMany(entry => entry.Errors)
                .Select(error => error.ErrorMessage)
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .Distinct()
                .ToList();

            TempData["Error"] = messages.Count == 0
                ? "Please review the submitted values and try again."
                : string.Join(" ", messages);
        }

        private IActionResult RedirectBack(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
