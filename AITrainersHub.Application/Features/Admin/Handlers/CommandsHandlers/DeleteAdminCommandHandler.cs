using System.Net;
using AITrainersHub.Application.Features.Admin.Commands;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AITrainersHub.Application.Features.Admin.Handlers.CommandsHandlers
{
    /// <summary>
    /// Deletes an administrator account while preventing self-deletion.
    /// </summary>
    public class DeleteAdminCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<DeleteAdminCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteAdminCommand request,
            CancellationToken cancellationToken)
        {
            if (request.AdminId == request.CurrentAdminId)
            {
                throw new ResultException(
                    "You cannot delete your own admin account.",
                    (int)HttpStatusCode.BadRequest);
            }

            var admin = await userManager.FindByIdAsync(request.AdminId.ToString());

            if (admin is null || !await userManager.IsInRoleAsync(admin, Helpers.AdminRole))
            {
                throw new ResultException(
                    "Admin account not found.",
                    (int)HttpStatusCode.NotFound);
            }

            var result = await userManager.DeleteAsync(admin);

            if (!result.Succeeded)
            {
                throw new ResultException(
                    string.Join(" ", result.Errors.Select(error => error.Description)),
                    (int)HttpStatusCode.BadRequest);
            }

            return Unit.Value;
        }
    }
}
