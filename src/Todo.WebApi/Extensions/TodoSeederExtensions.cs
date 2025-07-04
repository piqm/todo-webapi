using Microsoft.EntityFrameworkCore;

namespace Todo.WebApi.Extensions
{
    public static class TodoSeederExtensions
    {
        public static WebApplication TodoSeederDbContext<TContext>(this WebApplication app, Action<TContext> seed = null) where TContext : DbContext
        {
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<TContext>();

                    if (context != null)
                    {
                        context.Database.Migrate();
                        seed?.Invoke(context);
                    }
                }
                catch (Exception exception)
                {
                    //var logger = scope.ServiceProvider.GetService<ILogger<TContext>>();
                    //logger.LogError(exception, $"An error ocurred while migrating database for ${nameof(TContext)}");
                }

                return app;
            }
        }
    }
}
