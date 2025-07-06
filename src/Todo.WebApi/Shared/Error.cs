namespace Todo.WebApi.Shared
{
    /// <summary>
    /// Represents a concrete domain error.
    /// </summary>
    public sealed class Error : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public Error(string code, string message, ErrorType errorType)
        {
            Code = code;
            Message = message;
            Type = errorType;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        public ErrorType Type { get; }

        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }

        /// <summary>
        /// Gets the empty error instance.
        /// </summary>
        public static Error None => new Error(string.Empty, string.Empty, ErrorType.Failure);
        public static Error NullValue => new("Error.NullValue", "The specified result value is null.", ErrorType.Failure);
        public static Error ConditionNotMet => new("Error.ConditionNotMet", "The specified condition was not met.", ErrorType.Failure);
        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
        public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
        public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
    }
}
