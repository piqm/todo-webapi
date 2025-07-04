using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Tasks.Infrastructure;

namespace Todo.WebApi.Features.Tasks.UseCase
{
    internal sealed class DeleteTask(AppDbContext context)
    {
        public async Task<TodoTask> Handle(Guid id)
    {
        TodoTask? taks = await context.Tasks.GetById(id);

        if (taks is null)
        {
            throw new Exception("The taks was not found");
        }

        context.Tasks.Remove(taks);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("The taks not deleted", e);
        }

        return taks;
    }
}
}
