using AITrainerHub.Extensions;
using AITrainerHub.Request_Models;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Country.Commands;
using AITrainersHub.Application.Features.Country.Queries;
using AITrainersHub.Application.Features.PaymentMethod.Commands;
using AITrainersHub.Application.Features.PaymentMethod.Queries;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    public class ReferenceDataController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public ReferenceDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> PaymentMethods(CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new ReadPaymentMethodsQuery(), cancellationToken);

            ViewData["Title"] = "Reference Data — Payment Methods";
            ViewData["ReferenceTab"] = "payments";

            return View(new PaymentMethodsViewModel { Items = items });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaymentMethod(
            PaymentMethodFormRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(PaymentMethods));
            }

            try
            {
                await _mediator.Send(
                    new AddPaymentMethodCommand((request.Name ?? string.Empty).Trim()),
                    cancellationToken);

                TempData["Success"] = "Payment method added successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(PaymentMethods));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentMethod(
            PaymentMethodFormRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(PaymentMethods));
            }

            try
            {
                await _mediator.Send(
                    new UpdatePaymentMethodCommand(request.Id, (request.Name ?? string.Empty).Trim()),
                    cancellationToken);

                TempData["Success"] = "Payment method updated successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(PaymentMethods));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePaymentMethod(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeletePaymentMethodCommand(id), cancellationToken);
                TempData["Success"] = "Payment method deleted successfully.";
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(PaymentMethods));
        }

        [HttpGet]
        public async Task<IActionResult> Countries(CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new ReadCountriesQuery(), cancellationToken);

            ViewData["Title"] = "Reference Data — Countries";
            ViewData["ReferenceTab"] = "countries";

            return View(new CountriesViewModel { Items = items });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(
            CountryFormRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Countries));
            }

            try
            {
                await _mediator.Send(
                    new AddCountryCommand(
                        (request.NameAr ?? string.Empty).Trim(),
                        (request.NameEn ?? string.Empty).Trim(),
                        (request.IsoCode ?? string.Empty).Trim().ToUpperInvariant()),
                    cancellationToken);

                TempData["Success"] = "Country added successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Countries));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(
            CountryFormRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                SetModelStateError();
                return RedirectToAction(nameof(Countries));
            }

            try
            {
                await _mediator.Send(
                    new UpdateCountryCommand(
                        request.Id,
                        (request.NameAr ?? string.Empty).Trim(),
                        (request.NameEn ?? string.Empty).Trim(),
                        (request.IsoCode ?? string.Empty).Trim().ToUpperInvariant()),
                    cancellationToken);

                TempData["Success"] = "Country updated successfully.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Countries));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCountry(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeleteCountryCommand(id), cancellationToken);
                TempData["Success"] = "Country deleted successfully.";
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Countries));
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
    }
}
