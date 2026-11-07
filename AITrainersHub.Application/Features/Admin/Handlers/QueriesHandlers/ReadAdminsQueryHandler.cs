using AITrainersHub.Application.Features.Admin.DTOs;
using AITrainersHub.Application.Features.Admin.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AITrainersHub.Application.Features.Admin.Handlers.QueriesHandlers
{
    /// <summary>
    /// Reads all administrator accounts from ASP.NET Core Identity.
    /// </summary>
    public class ReadAdminsQueryHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<ReadAdminsQuery, List<AdminDto>>
    {
        public async Task<List<AdminDto>> Handle(
            ReadAdminsQuery request,
            CancellationToken cancellationToken)
        {
            var admins = await userManager.GetUsersInRoleAsync(Helpers.AdminRole);

            return admins
                .OrderBy(admin => admin.DisplayName ?? admin.Email)
                .Select(admin => new AdminDto
                {
                    Id = admin.Id,
                    Name = admin.DisplayName
                        ?? admin.UserName
                        ?? admin.Email
                        ?? "Unnamed admin",
                    Email = admin.Email ?? string.Empty,
                    CreatedAt = admin.CreatedAt,
                    IsActive = admin.IsActive
                })
                .ToList();
        }
    }
}
