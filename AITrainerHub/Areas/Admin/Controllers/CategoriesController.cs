using AITrainersHub.Application.Features.Categories.Commands;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainerHub.Request_Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryFormRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _mediator.Send(
                new AddCategoryCommands(request.Name, request.Description ?? string.Empty),
                cancellationToken);

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var category = await _mediator.Send(new ReadCategoryById(id), cancellationToken);

            return View(new CategoryFormRequest
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryFormRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _mediator.Send(
                new UpdateCategoryCommand(request.Id, request.Name, request.Description),
                cancellationToken);

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
