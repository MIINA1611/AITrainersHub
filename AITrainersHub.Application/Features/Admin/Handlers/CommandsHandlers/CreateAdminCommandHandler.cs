using System.Net;
using AITrainersHub.Application.Features.Admin.Commands;
using AITrainersHub.Application.Features.Admin.DTOs;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AITrainersHub.Application.Features.Admin.Handlers.CommandsHandlers
{
    /// <summary>
    /// Creates the Identity account and assigns the Admin role.
    /// </summary>
    public class CreateAdminCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<CreateAdminCommand, AdminDto>
    {
        public async Task<AdminDto> Handle(
            CreateAdminCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();

            if (await userManager.FindByEmailAsync(email) is not null)
            {
                throw new ResultException(
                    "An account with this email address already exists.",
                    (int)HttpStatusCode.Conflict);
            }

            var admin = new ApplicationUser
            {
                DisplayName = request.Name.Trim(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(admin, request.Password);

            if (!createResult.Succeeded)
            {
                throw new ResultException(
                    string.Join(" ", createResult.Errors.Select(error => error.Description)),
                    (int)HttpStatusCode.BadRequest);
            }

            var roleResult = await userManager.AddToRoleAsync(admin, Helpers.AdminRole);

            if (!roleResult.Succeeded)
            {
                // Do not leave behind an account that cannot access the Admin Area.
                await userManager.DeleteAsync(admin);

                throw new ResultException(
                    string.Join(" ", roleResult.Errors.Select(error => error.Description)),
                    (int)HttpStatusCode.InternalServerError);
            }

            return new AdminDto
            {
                Id = admin.Id,
                Name = admin.DisplayName ?? admin.Email ?? string.Empty,
                Email = admin.Email ?? string.Empty,
                CreatedAt = admin.CreatedAt,
                IsActive = admin.IsActive
            };
        }
    }
}
