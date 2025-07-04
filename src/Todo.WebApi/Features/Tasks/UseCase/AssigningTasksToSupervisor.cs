using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Tasks.Infrastructure;

namespace Todo.WebApi.Features.Tasks.UseCase
{
    internal sealed class AssigningTasksToSupervisor(AppDbContext context)
    {
        public sealed record Request(Guid Id, Guid? ReviewerId);

    public async Task<TodoTask> Handle(Request request)
    {
        TodoTask? task = await context.Tasks.GetById(request.Id);

        if (task is null)
        {
            throw new Exception("The taks was not found");
        }

        task.ReviewerId = request.ReviewerId;
        task.ModifiedOnUtc = DateTime.UtcNow;
        context.Tasks.Update(task);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("Error creating task", e);
        }

        return task;
    }
}
}
