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



            return builder;
        }
    }
}
