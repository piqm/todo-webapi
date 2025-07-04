using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Features.Users.Infrastructure;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class AssigningRolesToUser(AppDbContext context)
    {
        public sealed record Request(Guid userId, IEnumerable<Guid> roles);

    public async Task<User> Handle(Request request)
    {
        User? user = await context.Users.GetById(request.userId);

        if (user is null)
        {
            throw new Exception("The user was not found");
        }

        if (!user.UserRoles.Any())
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
            var userRolesList = user.UserRoles.ToList();

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

            user.UserRoles = userRolesList;
            context.Users.Update(user);
        }



        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("The user not updated", e);
        }

        return user;
    }
}
}
