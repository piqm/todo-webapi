using Todo.WebApi.Features.Roles;

namespace Todo.WebApi.Features.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailVerified { get; set; }
        //public IEnumerable<UserRole> UserRoles { get; set; } = [];

        public IEnumerable<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
