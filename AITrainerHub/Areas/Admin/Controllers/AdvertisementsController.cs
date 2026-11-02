using AITrainerHub.Extensions;
using AITrainerHub.Request_Models;
using AITrainersHub.Application.Features.Advertisement.Commands;
using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementsController : Controller
    {
        private readonly IMediator _mediator;

        public AdvertisementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var advertisements = await _mediator.Send(new ReadAdvertisementsQuery(), cancellationToken);

            ViewData["Title"] = "Advertisements";
            return View(advertisements);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Add Advertisement";

            return View(new AdvertisementFormRequest
            {
                CtaText = "Book now",
                Position = "Hero",
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AdvertisementFormRequest request,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Add Advertisement";

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _mediator.Send(
                    new AddAdvertisementCommand
                    {
                        Title = request.Title.Trim(),
                        Subtitle = NormalizeOptional(request.Subtitle),
                        CtaText = request.CtaText.Trim(),
                        CtaUrl = request.CtaUrl.Trim(),
                        Position = request.Position.Trim(),
                        IsActive = request.IsActive,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate
                    },
                    cancellationToken);

                TempData["Success"] = "Advertisement created successfully.";
                return RedirectToAction(nameof(Index));
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
            try
            {
                var advertisement = await _mediator.Send(new ReadAdvertisementByIdQuery(id), cancellationToken);

                ViewData["Title"] = "Edit Advertisement";
                return View(MapToForm(advertisement));
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            AdvertisementFormRequest request,
            CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            ViewData["Title"] = "Edit Advertisement";

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _mediator.Send(
                    new UpdateAdvertisementCommand
                    {
                        Id = id,
                        Title = request.Title.Trim(),
                        // Empty strings clear the optional text fields because the
                        // entity update method only keeps the previous value for null.
                        Subtitle = NormalizeOptional(request.Subtitle) ?? string.Empty,
                        CtaText = request.CtaText.Trim(),
                        CtaUrl = request.CtaUrl.Trim(),
                        Position = request.Position.Trim(),
                        IsActive = request.IsActive,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        UpdateSchedule = true
                    },
                    cancellationToken);

                TempData["Success"] = "Advertisement updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(request);
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var advertisement = await _mediator.Send(new ReadAdvertisementByIdQuery(id), cancellationToken);

                await _mediator.Send(
                    new UpdateAdvertisementCommand
                    {
                        Id = id,
                        IsActive = !advertisement.IsActive
                    },
                    cancellationToken);

                TempData["Success"] = advertisement.IsActive
                    ? "Advertisement deactivated successfully."
                    : "Advertisement activated successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeleteAdvertisementCommand(id), cancellationToken);
                TempData["Success"] = "Advertisement deleted successfully.";
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private static AdvertisementFormRequest MapToForm(AdvertisementDto advertisement) => new()
        {
            Id = advertisement.Id,
            Title = advertisement.Title,
            Subtitle = advertisement.Subtitle,
            CtaText = advertisement.CtaText,
            CtaUrl = advertisement.CtaUrl,
            Position = advertisement.Position,
            IsActive = advertisement.IsActive,
            StartDate = advertisement.StartDate,
            EndDate = advertisement.EndDate
        };

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
