using Microsoft.EntityFrameworkCore;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Shared;

namespace Todo.WebApi.Migrations.Seeders
{
    public static class TodoSeeder
    {
        public static async Task Seed(AppDbContext db)
        {

            var roles = await db.Roles.ToListAsync() ?? [];


            if (!roles.Any())
            {
                roles.Add(new Role() { Id = new Guid("74eddc48-9a0d-418f-b074-c3867db03b31"), Name = Constants.RoleAdministrador, Description = "Administrador" });
                roles.Add(new Role() { Id = new Guid("fab11349-cc21-4e73-ad24-27e8e78a4650"), Name = Constants.RoleSupervisor, Description = "Supervisor" });
                roles.Add(new Role() { Id = new Guid("219cb681-2ec3-4a9a-8c69-4d94b40a28fe"), Name = Constants.RoleEmpleado, Description = "Empleado" });


                db.Roles.AddRange(roles);

                await db.SaveChangesAsync();
            }
        }
    }
}
