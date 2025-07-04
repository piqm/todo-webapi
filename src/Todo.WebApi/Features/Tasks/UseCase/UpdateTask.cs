using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Tasks.Infrastructure;
using Todo.WebApi.Shared;

namespace Todo.WebApi.Features.Tasks.UseCase
{
    internal sealed class UpdateTask(AppDbContext context)
    {
        public sealed record Request(
            Guid Id,
            string Description,
            string Type,
            Shared.TaskStatus Status,
            Guid? ReviewerId,
            Guid? EmployeeId,
            TaskPriority Priority);

    public async Task<TodoTask> Handle(Request request)
    {

        TodoTask? task = await context.Tasks.GetById(request.Id);

        if (task is null)
        {
            throw new Exception("The taks was not found");
        }

        task.Description = request.Description;
        task.Type = request.Type;
        task.Status = request.Status;
        task.ReviewerId = request.ReviewerId;
        task.EmployeeId = request.EmployeeId;
        task.Priority = request.Priority;
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
