using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Features.Users.Infrastructure;
using Todo.WebApi.Shared;
using Todo.WebApi.Shared.Results;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class AssigningRolesToUser(AppDbContext context)
    {
        public sealed record Request(Guid userId, IEnumerable<Guid> roles);
        public sealed record UserResponse(
            Guid Id,
            string FirstName,
            string LastName,
            string Email,
            bool EmailVerified,
            Role[] Roles);

        public async Task<Result<UserResponse>> Handle(Request request)
        {
            User? user = await context.Users.GetById(request.userId);

            if (user is null)
            {
                return Result.Failure<UserResponse>(
                        new Error[]
                        {
                           new ("User.Validation", "The user was not found", ErrorType.Validation)
                        });
            }

            if (!user.Roles.Any())
            {
                var userRoles = request.roles.Select(roleId => new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = roleId
                }).ToList();

                await context.UserRoles.AddRangeAsync(userRoles);
            }
            else
            {
                // Convert UserRoles to a List to use RemoveAll
                var userRolesList = user.Roles.ToList();

                // Remove roles that are not in the request
                userRolesList.RemoveAll(ur => !request.roles.Contains(ur.RoleId));

                // Add new roles that are in the request but not in the user's current roles
                foreach (var roleId in request.roles)
                {
                    if (!userRolesList.Any(ur => ur.RoleId == roleId))
                    {
                        userRolesList.Add(new UserRole
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            RoleId = roleId
                        });
                    }
                }

                user.Roles = userRolesList;
                context.Users.Update(user);
            }



            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
                when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
            {


                return Result.Failure<UserResponse>(
                       new Error[]
                       {
                           new ("User.BadRequest", "The user not updated", ErrorType.BadRequest)
                       });

            }

            var userResponse = new UserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.EmailVerified,
                user.Roles.Select(ur => ur.Role).ToArray()
            );

            return userResponse;
        }
    }
}
