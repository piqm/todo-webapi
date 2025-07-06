using Todo.WebApi.Features.Roles.UseCase;

namespace Todo.WebApi.Features.Roles
{
    internal static class RolesEndpoints
    {
        private const string Tag = "Roles";
        public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
        {
            builder.MapPost("roles/create", async (CreateRole.Request request, CreateRole useCase) =>
                await useCase.Handle(request))
                .WithTags(Tag)
                .RequireAuthorization();

            builder.MapGet("roles", async (GetRoles useCase) =>
            {
                var response = await useCase.Handle();
                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest("Verification token expired");
            })
            .WithTags(Tag)
            .RequireAuthorization();

            return builder;
        }
    }
}
