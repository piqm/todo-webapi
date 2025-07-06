using Microsoft.EntityFrameworkCore;
using Todo.WebApi.Database;
using Todo.WebApi.Shared.Results;

namespace Todo.WebApi.Features.Roles.UseCase
{


    internal sealed class GetRoles(AppDbContext context)
    {
        public sealed record RoleResponse(
            Guid Id,
            string Name,
            string Description
            );

        public async Task<Result<IEnumerable<RoleResponse>>> Handle()
        {
            var roles = await context.Roles
                .Select(u => new RoleResponse(u.Id, u.Name, u.Description))
                .ToListAsync();

            return roles;
        }
    }

}
