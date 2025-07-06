using Microsoft.EntityFrameworkCore;

namespace Todo.WebApi.Features.Users.Infrastructure
{
    internal static class UserDbSetExtensions
    {
        public static async Task<bool> Exists(this DbSet<User> users, string email)
        {
            return await users.AnyAsync(u => u.Email == email);
        }

        public static async Task<bool> Exists(this DbSet<User> users, Guid id)
        {
            return await users.AnyAsync(u => u.Id == id);
        }

        public static async Task<User?> GetByEmail(this DbSet<User> users, string email)
        {
            return await users
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email);
        }
        public static async Task<User?> GetById(this DbSet<User> users, Guid id)
        {
            return await users
                .Include(u => u.Roles)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
