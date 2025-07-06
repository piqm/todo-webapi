

using Microsoft.AspNetCore.Mvc;
using Todo.WebApi.Features.Tasks.UseCase;
using Todo.WebApi.Features.Users.UseCase;

namespace Todo.WebApi.Features.Tasks
{
    internal static class TasksEndpoints
    {
        private const string Tag = "Taks";
        public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
        {
            builder.MapGet("tasks/user/{id:guid}", async (
                Guid id,
                GetTasks useCase,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var result = await useCase.Handle(id, pageNumber, pageSize);
                

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Errors);
                }

                return Results.Ok(result.Value);


            })
            .WithTags(Tag)
            .RequireAuthorization();

            builder.MapPost("tasks", async (CreateTask.Request request, CreateTask useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();

            builder.MapPatch("tasks", async (UpdateTask.Request request, UpdateTask useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();

            builder.MapPatch("tasks/status", async (UpdateTaskStatus.Request request, UpdateTaskStatus useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();


            builder.MapPatch("tasks/assigning/supervisor", async (AssigningTasksToSupervisor.Request request, AssigningTasksToSupervisor useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();

            builder.MapPatch("tasks/assigning/employee", async (AssigningTasksToEmployee.Request request, AssigningTasksToEmployee useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();

            builder.MapDelete("tasks/{id:guid}", async (Guid id, DeleteTask useCase) =>
              await useCase.Handle(id))
              .WithTags(Tag)
              .RequireAuthorization();

            return builder;
        }
    }
}
