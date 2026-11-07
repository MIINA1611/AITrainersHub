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
    /// Activates or deactivates an administrator account while protecting the
    /// currently signed-in administrator from locking themselves out.
    /// </summary>
    public class SetAdminStatusCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<SetAdminStatusCommand, Unit>
    {
        public async Task<Unit> Handle(
            SetAdminStatusCommand request,
            CancellationToken cancellationToken)
        {
            var admin = await userManager.FindByIdAsync(request.AdminId.ToString());

            if (admin is null || !await userManager.IsInRoleAsync(admin, Helpers.AdminRole))
            {
                throw new ResultException(
                    "Admin account not found.",
                    (int)HttpStatusCode.NotFound);
            }

            if (!request.IsActive && admin.Id == request.CurrentAdminId)
            {
                throw new ResultException(
                    "You cannot deactivate your own admin account.",
                    (int)HttpStatusCode.BadRequest);
            }

            admin.IsActive = request.IsActive;
            admin.LockoutEnabled = true;
            admin.LockoutEnd = request.IsActive ? null : DateTimeOffset.MaxValue;

            var result = await userManager.UpdateAsync(admin);

            if (!result.Succeeded)
            {
                throw new ResultException(
                    string.Join(" ", result.Errors.Select(error => error.Description)),
                    (int)HttpStatusCode.BadRequest);
            }

            // Invalidate any authentication cookie that is already signed in.
            await userManager.UpdateSecurityStampAsync(admin);

            return Unit.Value;
        }
    }
}
