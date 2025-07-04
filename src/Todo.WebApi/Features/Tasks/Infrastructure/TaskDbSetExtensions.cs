using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Todo.WebApi.Features.Users;

namespace Todo.WebApi.Features.Tasks.Infrastructure
{
    internal static class TaskDbSetExtensions
    {
        public static async Task<TodoTask?> GetById(this DbSet<TodoTask> tasks, Guid id)
        {
            return await tasks.SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
