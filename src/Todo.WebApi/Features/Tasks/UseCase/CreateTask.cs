using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Shared;

namespace Todo.WebApi.Features.Tasks.UseCase
{
    internal sealed class CreateTask(AppDbContext context)
    {
        public sealed record Request(string Description, string Type, Shared.TaskStatus Status, Guid? ReviewerId, Guid? EmployeeId, TaskPriority Priority);

    public async Task<TodoTask> Handle(Request request)
    {
        var todoTask = new TodoTask
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Type = request.Type,
            Status = request.Status,
            ReviewerId = request.ReviewerId,
            EmployeeId = request.EmployeeId,
            Priority = request.Priority,
            CreatedOnUtc = DateTime.UtcNow,
        };

        context.Tasks.Add(todoTask);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("Error creating taks", e);
        }

        return todoTask;
    }
}
}
