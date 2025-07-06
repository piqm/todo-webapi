using Microsoft.EntityFrameworkCore;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Users;
using Todo.WebApi.Shared;
using Todo.WebApi.Shared.Results;

namespace Todo.WebApi.Features.Tasks.UseCase
{
    internal sealed class GetTasks(AppDbContext context)
    {
        public sealed record TaskResponse(
            Guid Id,
            string Description,
            string Type,
            Shared.TaskStatus Status,
            string StatusDescription,
            Guid? ReviewerId,
            User? Reviewer,
            Guid? EmployeeId,
            User? Employee,
            TaskPriority Priority,
            string PriorityDescription
        );

        public sealed record PaginatedTaskResponse(
            IEnumerable<TaskResponse> Tasks,
            int TotalCount,
            int PageNumber,
            int PageSize,
            int TotalPages,
            bool HasPreviousPage,
            bool HasNextPage
        );

        public async Task<Result<PaginatedTaskResponse>> Handle(
            Guid userId,
            int pageNumber = 1,
            int pageSize = 10
            )
        {

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limitar tamaño máximo

            var userIsAdmin = context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .Where(x => x.Id == userId && x.Roles.Any(r => r.Role.Name == Constants.RoleAdministrador))
                .Any();

            var queryBase = context.Tasks
                .Include(t => t.Reviewer)
                .Include(t => t.Employee);

            var query = userIsAdmin ? queryBase : context.Tasks
                .Where(t => t.EmployeeId == userId || t.ReviewerId == userId);

            // Obtener el conteo total antes de aplicar paginación
            var totalCount = await query.CountAsync();

            // Aplicar paginación
            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TaskResponse(
                    t.Id,
                    t.Description,
                    t.Type,
                    t.Status,
                    t.Status.ToString(),
                    t.ReviewerId,
                    t.Reviewer,
                    t.EmployeeId,
                    t.Employee,
                    t.Priority,
                    t.Priority.ToString()
                ))
                .ToListAsync();

            // Calcular información de paginación
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasPreviousPage = pageNumber > 1;
            var hasNextPage = pageNumber < totalPages;

            return new PaginatedTaskResponse(
                tasks,
                totalCount,
                pageNumber,
                pageSize,
                totalPages,
                hasPreviousPage,
                hasNextPage

            );

        }
    }
}
