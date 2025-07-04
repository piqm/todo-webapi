using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Users.Infrastructure;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class UpdateUser(AppDbContext context)
    {
        public sealed record Request(Guid Id, string Email, string FirstName, string LastName);

    public async Task<User> Handle(Request request)
    {
        User? user = await context.Users.GetById(request.Id);

        if (user is null)
        {
            throw new Exception("The user was not found");
        }

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        context.Users.Update(user);

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
