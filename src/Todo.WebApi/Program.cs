using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo.WebApi.Database;
using Todo.WebApi.Extensions;
using Todo.WebApi.Features.Users.Infrastructure;
using Todo.WebApi.Features.Users;
using Todo.WebApi.Features.Tasks;
using Todo.WebApi.Migrations.Seeders;
using Todo.WebApi.Features.Tasks.UseCase;
using Todo.WebApi.Features.Users.UseCase;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Features.Roles.UseCase;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Database")).UseSnakeCaseNamingConvention());

builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddScoped<EmailVerificationLinkFactory>();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddFluentEmail(builder.Configuration["Email:SenderEmail"], builder.Configuration["Email:Sender"])
    .AddSmtpSender(builder.Configuration["Email:Host"], builder.Configuration.GetValue<int>("Email:Port"));

//builder.Services.AddAuthorization();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmpleadoOnly", policy => policy.RequireRole("empleado"));
    options.AddPolicy("AdministradorOnly", policy => policy.RequireRole("administrador"));
    options.AddPolicy("SupervisorOnly", policy => policy.RequireRole("supervisor"));
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<LoginUser>();
builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<GetUser>();
builder.Services.AddScoped<GetUsers>();
builder.Services.AddScoped<UpdateUser>();
builder.Services.AddScoped<DeleteUser>();
builder.Services.AddScoped<VerifyEmail>();
builder.Services.AddScoped<AssigningRolesToUser>();


builder.Services.AddScoped<CreateRole>();
builder.Services.AddScoped<GetRoles>();

builder.Services.AddScoped<GetTasks>();
builder.Services.AddScoped<CreateTask>();
builder.Services.AddScoped<DeleteTask>();
builder.Services.AddScoped<UpdateTask>();


builder.Services.AddScoped<UpdateTaskStatus>();
builder.Services.AddScoped<AssigningTasksToEmployee>();
builder.Services.AddScoped<AssigningTasksToSupervisor>();


WebApplication app = builder.Build();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.TodoSeederDbContext<AppDbContext>(db => TodoSeeder.Seed(db).Wait());

UserEndpoints.Map(app);
RolesEndpoints.Map(app);
TasksEndpoints.Map(app);

app.UseAuthentication();

app.UseAuthorization();


app.Run();
