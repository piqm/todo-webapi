using System.ComponentModel.DataAnnotations;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Roles;
using Todo.WebApi.Features.Users.Infrastructure;
using Todo.WebApi.Shared;
using Todo.WebApi.Shared.Results;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class LoginUser(AppDbContext context, PasswordHasher passwordHasher, TokenProvider tokenProvider)
    {
        public sealed record Request(string Email, string Password);
        public sealed record LoginUserResponse(Guid Id, string Email, string FirstName, string LastName, IEnumerable<Role> Roles);
        public sealed record LoginResponse(LoginUserResponse user, string Token);

        public async Task<Result<LoginResponse>> Handle(Request request)
        {
            User? user = await context.Users.GetByEmail(request.Email);

            if (user is null || !user.EmailVerified)
            {
                // Wrap the Error instance in a collection to match the expected type  
                return Result.Failure<LoginResponse>(
                    new Error[]
                    {
                           new ("User.Validation", "The user was not found", ErrorType.Validation)
                    });
            }

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                return Result.Failure<LoginResponse>(
                    new Error[]
                    {
                           new ("User.Validation", "The password is incorrect", ErrorType.Validation)
                    });
            }

            var useRole = user.Roles?.FirstOrDefault();
            var useRoles = useRole is not null
                ? [useRole.Role]
                : new List<Role>();
            string token = tokenProvider.Create(user);
            var userResponse = new LoginUserResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                useRoles
            );

            return Result.Success(new LoginResponse(userResponse, token));

        }
    }
}
