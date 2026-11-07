using AITrainerHub.Extensions;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Admin.Commands;
using AITrainersHub.Application.Features.Admin.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    public class AdminsController : AdminBaseController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminsController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var admins = await _mediator.Send(new ReadAdminsQuery(), cancellationToken);
            var currentUser = await _userManager.GetUserAsync(User);
            var currentAdminId = currentUser?.Id ?? Guid.Empty;

            var model = new AdminsIndexViewModel
            {
                CurrentAdminId = currentAdminId,
                Admins = admins
                    .Select(admin => new AdminRowViewModel
                    {
                        Id = admin.Id,
                        DisplayName = admin.Name,
                        Email = admin.Email,
                        RegisteredAt = admin.CreatedAt,
                        IsActive = admin.IsActive,
                        IsCurrentUser = admin.Id == currentAdminId
                    })
                    .ToList()
            };

            ViewData["Title"] = "Manage Admins";
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Admin";
            return View(new CreateAdminViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateAdminViewModel model,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Create Admin";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var admin = await _mediator.Send(
                    new CreateAdminCommand
                    {
                        Name = model.Name.Trim(),
                        Email = model.Email.Trim(),
                        Password = model.Password
                    },
                    cancellationToken);

                TempData["Success"] = $"Admin account for {admin.Name} was created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(model);
            }
            catch (ResultException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
            {
                return Challenge();
            }

            var target = await _userManager.FindByIdAsync(id.ToString());
            if (target is null)
            {
                TempData["Error"] = "Admin account not found.";
                return RedirectToAction(nameof(Index));
            }

            if (target.Id == currentUser.Id && target.IsActive)
            {
                TempData["Error"] = "You cannot deactivate your own admin account.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _mediator.Send(
                    new SetAdminStatusCommand
                    {
                        AdminId = target.Id,
                        CurrentAdminId = currentUser.Id,
                        IsActive = !target.IsActive
                    },
                    cancellationToken);

                TempData["Success"] = target.IsActive
                    ? "Admin account deactivated successfully."
                    : "Admin account activated successfully.";
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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
            {
                return Challenge();
            }

            if (id == currentUser.Id)
            {
                TempData["Error"] = "You cannot delete your own admin account.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _mediator.Send(
                    new DeleteAdminCommand
                    {
                        AdminId = id,
                        CurrentAdminId = currentUser.Id
                    },
                    cancellationToken);

                TempData["Success"] = "Admin account deleted successfully.";
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
    }
}
