namespace BookStore.SharedKernel.Abstractions.Errors;

public record Error(string Code, string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);



    public static readonly Error NullValue = new("General.Null","Null value was provided",ErrorType.Failure);


    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);


    public static Error Validation(string code = "General.Validation", string description = "A validation error has occurred.")
    {
        return new Error(code, description, ErrorType.Validation);
    }

    public static Error Unexpected(string code = "General.Unexpected", string description = "An unexpected error has occurred.")
    {
        return new Error(code, description, ErrorType.Unexpected);
    }


    public static Error Unauthorized(string code = "General.Unauthorized", string description = "Unauthorized access.") =>
        new(code, description, ErrorType.Forbidden);

    public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden access.") =>
        new(code, description, ErrorType.Forbidden);


    public static Error Custom(int type, string code, string description)
    {
        return new Error(code, description, (ErrorType)type);
    }

}



