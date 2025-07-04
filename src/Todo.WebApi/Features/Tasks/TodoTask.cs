using System.ComponentModel;
using Todo.WebApi.Features.Users;
using Todo.WebApi.Shared;

namespace Todo.WebApi.Features.Tasks
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        [DefaultValue(Shared.TaskStatus.Pending)]
        public Shared.TaskStatus Status { get; set; } = Shared.TaskStatus.Pending;
        public Guid? ReviewerId { get; set; }
        public User? Reviewer { get; set; }
        public Guid? EmployeeId { get; set; }
        public User? Employee { get; set; }

        [DefaultValue(TaskPriority.Low)]
        public TaskPriority Priority { get; set; } = TaskPriority.Low;

        public string? CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }

    }
}
