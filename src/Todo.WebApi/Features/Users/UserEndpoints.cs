using Microsoft.AspNetCore.Mvc;
using Todo.WebApi.Features.Users.UseCase;

namespace Todo.WebApi.Features.Users
{
    internal static class UserEndpoints
    {
        private const string Tag = "Users";
        public const string VerifyEmail = "VerifyEmail";

        public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
        {
            builder.MapPost("users/register", async (RegisterUser.Request request, RegisterUser useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag);

            builder.MapPost("users/login", async (LoginUser.Request request, LoginUser useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag);

            builder.MapPatch("users", async (UpdateUser.Request request, UpdateUser useCase) =>
              await useCase.Handle(request))
              .WithTags(Tag)
              .RequireAuthorization();

            builder.MapPost("users/roles", async (AssigningRolesToUser.Request request, AssigningRolesToUser useCase) =>
              await useCase.Handle(request))
              .WithTags(Tag)
              .RequireAuthorization();

            builder.MapDelete("users/{id:guid}", async (Guid id, DeleteUser useCase) =>
              await useCase.Handle(id))
              .WithTags(Tag)
              .RequireAuthorization();

            builder.MapGet("users/verify-email", async (Guid token, VerifyEmail useCase) =>
            {
                bool success = await useCase.Handle(token);

                return success ? Results.Ok() : Results.BadRequest("Verification token expired");
            })
            .WithTags(Tag)
            .WithName(VerifyEmail);

            builder.MapGet("users/{id:guid}", async (Guid id, GetUser useCase) =>
            {
                GetUser.UserResponse? user = await useCase.Handle(id);

                return user is not null ? Results.Ok(user) : Results.NotFound();
            })
            .WithTags(Tag)
            .RequireAuthorization();

            builder.MapGet("users", async (GetUsers useCase) =>
            {
                var response  = await useCase.Handle();

                //return user is not null ? Results.Ok(user) : Results.NotFound();
                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest("Verification token expired");
            })
            .WithTags(Tag)
            .RequireAuthorization();

            return builder;
        }
    }
}
