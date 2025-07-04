using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;

namespace Todo.WebApi.Features.Roles.UseCase
{
    internal sealed class CreateRole(AppDbContext context)
    {
        public sealed record Request(string Name, string Description);

    public async Task<Role> Handle(Request request)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
        };

        context.Roles.Add(role);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("Error creating role", e);
        }

        return role;
    }
}
}
