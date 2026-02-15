namespace SharedKernel.Primitives.Result;

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    NullValue,
    None,
}
