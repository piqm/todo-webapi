using Microsoft.EntityFrameworkCore;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Shared.Results;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class GetUsers(AppDbContext context)
    {
        public sealed record UserResponse(
            Guid Id, 
            string FirstName, 
            string LastName, 
            string Email, 
            bool EmailVerified,
            Role[] Roles);

        public async Task<Result<IEnumerable<UserResponse>>> Handle()
        {
            var users = await context.Users
                .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                //.Where(u => u.Id == userId)
                //.Select(u => new UserResponse(u.Id, u.FirstName, u.LastName, u.Email, u.EmailVerified))
                .ToListAsync();

            var userResponse = users.Select(u => new UserResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.EmailVerified,
                u.Roles.Select(ur => ur.Role).ToArray()
            ));

            return Result.Success(userResponse); 
        }
    }
}
