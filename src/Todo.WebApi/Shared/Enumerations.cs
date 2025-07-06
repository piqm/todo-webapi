namespace Todo.WebApi.Shared
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Done,
        Cancelled
    }

    public enum ErrorType
    {
        Failure = 0,
        BadRequest = 1,
        Validation = 2,
        NotFound = 3,
        Conflict = 4
    }
}
