using AITrainerHub.Extensions;
using AITrainerHub.Request_Models.Leader;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Models;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Areas.Leader.Controllers
{
    public class OpportunitiesController : LeaderControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<OpportunityFormRequest> _validator;

        public OpportunitiesController(
            IMediator mediator,
            IValidator<OpportunityFormRequest> validator,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(userManager, signInManager)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Leader" });
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewData["Title"] = "New Opportunity";
            return View(await BuildFormAsync(null, false, cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(Prefix = "Form")] OpportunityFormRequest form,
            CancellationToken cancellationToken)
        {
            ViewData["Title"] = "New Opportunity";

            var validation = await _validator.ValidateAsync(form, cancellationToken);
            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState, "Form");
                return View(await BuildFormAsync(form, false, cancellationToken));
            }

            try
            {
                await _mediator.Send(
                    new AddWorkOpportunityCommand(
                        LeaderId,
                        form.PlatformId,
                        form.Title.Trim(),
                        form.DescriptionAr.Trim(),
                        form.DescriptionEn.Trim(),
                        form.WhatsAppLink.Trim()),
                    cancellationToken);

                TempData["Success"] = "Opportunity submitted for review. It will appear publicly after administrator approval.";
                return RedirectToAction("Index", "Home", new { area = "Leader" });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(await BuildFormAsync(form, false, cancellationToken));
            }
            catch (ResultException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(await BuildFormAsync(form, false, cancellationToken));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var opportunity = await GetOwnedOpportunityAsync(id, cancellationToken);

            if (opportunity is null)
            {
                TempData["Error"] = "Opportunity not found, or you do not have permission to edit it.";
                return RedirectToAction("Index", "Home", new { area = "Leader" });
            }

            ViewData["Title"] = "Edit Opportunity";

            var form = new OpportunityFormRequest
            {
                Id = opportunity.Id,
                PlatformId = opportunity.PlatformId,
                Title = opportunity.Title,
                DescriptionAr = opportunity.DescriptionAr,
                DescriptionEn = opportunity.DescriptionEn,
                WhatsAppLink = opportunity.WhatsAppLink,
                IsActive = opportunity.IsActive
            };

            return View(await BuildFormAsync(form, true, cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind(Prefix = "Form")] OpportunityFormRequest form,
            CancellationToken cancellationToken)
        {
            if (id != form.Id)
            {
                return BadRequest();
            }

            var opportunity = await GetOwnedOpportunityAsync(id, cancellationToken);
            if (opportunity is null)
            {
                return Forbid();
            }

            ViewData["Title"] = "Edit Opportunity";

            var validation = await _validator.ValidateAsync(form, cancellationToken);
            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState, "Form");
                return View(await BuildFormAsync(form, true, cancellationToken));
            }

            try
            {
                await _mediator.Send(
                    new UpdateWorkOpportunityCommand
                    {
                        Id = form.Id,
                        PlatformId = form.PlatformId,
                        Title = form.Title.Trim(),
                        DescriptionAr = form.DescriptionAr.Trim(),
                        DescriptionEn = form.DescriptionEn.Trim(),
                        WhatsAppLink = form.WhatsAppLink.Trim(),
                        // Leaders can hide an opportunity, but only an admin can approve it.
                        IsApproved = null,
                        IsActive = form.IsActive
                    },
                    cancellationToken);

                TempData["Success"] = form.IsActive
                    ? "Opportunity updated successfully."
                    : "Opportunity updated and hidden from the public board.";

                return RedirectToAction("Index", "Home", new { area = "Leader" });
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(await BuildFormAsync(form, true, cancellationToken));
            }
            catch (ResultException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(await BuildFormAsync(form, true, cancellationToken));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var opportunity = await GetOwnedOpportunityAsync(id, cancellationToken);

            if (opportunity is null)
            {
                TempData["Error"] = "Opportunity not found, or you do not have permission to delete it.";
                return RedirectToAction("Index", "Home", new { area = "Leader" });
            }

            try
            {
                await _mediator.Send(new DeleteWorkOpportunityCommand(id), cancellationToken);
                TempData["Success"] = "Opportunity deleted successfully.";
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction("Index", "Home", new { area = "Leader" });
        }

        private async Task<LeaderOpportunityFormViewModel> BuildFormAsync(
            OpportunityFormRequest? form,
            bool isEdit,
            CancellationToken cancellationToken)
        {
            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);

            return new LeaderOpportunityFormViewModel
            {
                Form = form ?? new OpportunityFormRequest { IsActive = true },
                Platforms = platforms
                    .OrderBy(platform => platform.Name)
                    .ToList(),
                IsEdit = isEdit,
                FormAction = isEdit ? "Edit" : "Create",
                FormTitle = isEdit ? "Edit opportunity" : "New opportunity",
                FormSubtitle = isEdit
                    ? "Update the details below. Approval state is managed by the administrator."
                    : "Share a real opportunity with the community. New opportunities are reviewed before they go live."
            };
        }

        private async Task<WorkOpportunityDto?> GetOwnedOpportunityAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var opportunity = await _mediator.Send(
                    new ReadWorkOpportunityByIdQuery(id),
                    cancellationToken);

                return opportunity.LeaderId == LeaderId ? opportunity : null;
            }
            catch (ResultException)
            {
                return null;
            }
        }
    }
}
