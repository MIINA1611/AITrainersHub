using AITrainersHub.Application.Features.Admin.DTOs;
using MediatR;

namespace AITrainersHub.Application.Features.Admin.Queries
{
    /// <summary>
    /// Reads every account that belongs to the Admin role.
    /// </summary>
    public class ReadAdminsQuery : IRequest<List<AdminDto>>
    {
    }
}
