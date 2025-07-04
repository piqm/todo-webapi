using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Users.Infrastructure;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class DeleteUser(AppDbContext context)
    {
        public async Task<User> Handle(Guid id)
    {
        User? user = await context.Users.GetById(id);

        if (user is null)
        {
            throw new Exception("The user was not found");
        }

        context.Users.Remove(user);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("The user not deleted", e);
        }

        return user;
    }
}
}
