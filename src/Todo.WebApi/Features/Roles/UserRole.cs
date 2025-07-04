using Todo.WebApi.Features.Users;

namespace Todo.WebApi.Features.Roles
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
