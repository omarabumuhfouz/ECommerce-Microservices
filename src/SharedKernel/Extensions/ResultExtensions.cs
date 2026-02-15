using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Shared;

public static class ResultExtensions
{
    public static Result FailIf(this Result result, bool condition, Error error)
    {
        // 1. If we already failed in a previous step, stop and pass the old error down.
        if (result.IsFailure)
            return result;

        // 2. If the "Bad Condition" is true, fail now with the new error.
        if (condition)
            return Result.Failure(error);

        // 3. Otherwise, keep going (Success).
        return Result.Success();
    }

    /// <summary>
    /// Converts an Error directly to a Result.Failure and logs the warning.
    /// Useful for returning DomainErrors directly: return DomainErrors.Payment.NotFound.Log(...);
    /// </summary>
    public static Result Log(this Error error, ILogger logger, string message, params object[] args)
    {
        Result result = error;

        return result.LogIfFailure(logger, message, args);
    }

    public static Result LogIfFailure(this Result result, ILogger logger, string message, params object[] args)
    {
        if (result.IsFailure)
        {
            LogFailure(logger, result.TopError, message, args);
        }

        return result;
    }

    public static Result<T> LogIfFailure<T>(this Result<T> result, ILogger logger, string message, params object[] args)
    {
        if (result.IsFailure)
        {
            LogFailure(logger, result.TopError, message, args);
        }

        return result;
    }

    private static void LogFailure(ILogger logger, Error error, string message, object[] args)
    {
        if (logger == null) return;

        string template = $"{message} {{ErrorCode}} - {{ErrorDescription}}";

        var logArgs = new List<object>(args)
            {
                error.Code,
                error.Message
            };

        logger.LogWarning(template, logArgs.ToArray());
    }

    /// <summary>
    /// BIND: Used to chain operations that also return a Result.
    /// If the current result is a failure, it skips the func and returns the failure.
    /// </summary>
    public static Result<TNextValue> Bind<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, Result<TNextValue>> func)
    {
        return result.IsSuccess ? func(result.Value) : Result.Failure<TNextValue>(result.TopError);
    }

    public static async Task<Result<TNextValue>> Bind<TValue, TNextValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result<TNextValue>>> func)
    {
        var result = await resultTask;
        return result.IsSuccess ? await func(result.Value) : Result.Failure<TNextValue>(result.TopError);
    }

    /// <summary>
    /// MAP: Used to transform the value inside a success result.
    /// The transformation itself doesn't return a Result (it's "plain" logic).
    /// </summary>
    public static Result<TNextValue> Map<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, TNextValue> func)
    {
        return result.IsSuccess ? Result.Success(func(result.Value)) : Result.Failure<TNextValue>(result.TopError);
    }

    /// <summary>
    /// TAP: Executes a side effect (like logging or a void method) only if success.
    /// It returns the original result unchanged.
    /// </summary>
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }
        return result;
    }

    public static async Task<Result<TValue>> Tap<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> func)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await func(result.Value);
        }
        return result;
    }
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, Result> predicate)
    {
        if (result.IsFailure) return result;
        var predicateResult = predicate(result.Value);
        return predicateResult.IsSuccess ? result : Result.Failure<T>(predicateResult.TopError);
    }

    // 1. ToResult: The Entry Point


    // 2. Ensure: The "Check" (Stays on the same object)
    public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, Result> predicate)
    {
        var result = await resultTask;
        if (result.IsFailure) return result;

        var predicateResult = predicate(result.Value);
        return predicateResult.IsSuccess ? result : Result.Failure<T>(predicateResult.TopError);
    }

    // 3. Bind: The "Switch" (Changes the object type)
    public static async Task<Result<TNext>> Bind<T, TNext>(this Task<Result<T>> resultTask, Func<T, Result<TNext>> func)
    {
        var result = await resultTask;
        return result.IsSuccess ? func(result.Value) : Result.Failure<TNext>(result.TopError);
    }

    // // 4. Tap: The "Side Effect" (For SaveChanges, Logging, etc.)
    // public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Func<T, Task> func)
    // {
    //     var result = await resultTask;
    //     if (result.IsSuccess) await func(result.Value);
    //     return result;
    // }

    // 5. Map: The "Exit" (Final transformation)
    public static async Task<Result<TNext>> Map<T, TNext>(this Task<Result<T>> resultTask, Func<T, TNext> func)
    {
        var result = await resultTask;
        return result.IsSuccess ? Result.Success(func(result.Value)) : Result.Failure<TNext>(result.TopError);
    }

    // Sync version: Result<T> -> (T -> bool) -> Error -> Result<T>
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }

    // Async version: Task<Result<T>> -> (T -> bool) -> Error -> Task<Result<T>>
    public static async Task<Result<T>> Ensure<T>(
        this Task<Result<T>> resultTask,
        Func<T, bool> predicate,
        Error error)
    {
        var result = await resultTask;
        if (result.IsFailure) return result;

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }

    public static Result Combine<T>(this IEnumerable<T> items, Func<T, Result> func)
    {
        foreach (var item in items)
        {
            var result = func(item);
            if (result.IsFailure) return result; // Derail immediately
        }
        return Result.Success();
    }

    public static Result Bind<T>(this Result<T> result, Func<T, Result> func)
    {
        if (result.IsFailure)
            return Result.Failure(result.TopError);

        return func(result.Value);
    }

    public static Result<TNextValue> Map<TNextValue>(
        this Result result,
        Func<TNextValue> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure<TNextValue>(result.TopError);
        }

        return Result.Success(func());
    }

    public static async Task<Result<List<TOut>>> Combine<TIn, TOut>(
        this IEnumerable<TIn> items,
        Func<TIn, Task<Result<TOut>>> func)
    {
        var results = new List<TOut>();
        foreach (var item in items)
        {
            var result = await func(item);
            if (result.IsFailure) return Result.Failure<List<TOut>>(result.TopError);
            results.Add(result.Value);
        }
        return Result.Success(results);
    }

    public static Result<T> Ensure<T>(
        this Result<T> result,
        bool condition,
        Error error)
    {
        if (result.IsFailure) return result;

        return condition ? result : Result.Failure<T>(error);
    }

    public static Result Ensure(
        this Result result,
        bool condition,
        Error error)
    {
        if (result.IsFailure) return result;

        return condition ? Result.Success() : Result.Failure(error);
    }

    /// <summary>
    /// Executes the action if the result is a failure.
    /// </summary>
    public static Result<T> TapError<T>(this Result<T> result, Action<Error> action)
    {
        if (result.IsFailure)
        {
            action(result.TopError);
        }

        return result;
    }

    /// <summary>
    /// Executes the action if the non-generic result is a failure.
    /// </summary>
    public static Result TapError(this Result result, Action<Error> action)
    {
        if (result.IsFailure)
        {
            action(result.TopError);
        }

        return result;
    }

    /// <summary>
    /// Executes an asynchronous action if the result is a failure.
    /// </summary>
    public static async Task<Result<T>> TapError<T>(this Task<Result<T>> resultTask, Func<Error, Task> func)
    {
        Result<T> result = await resultTask;

        if (result.IsFailure)
        {
            await func(result.TopError);
        }

        return result;
    }

    public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> action)
    {
        var result = await resultTask;

        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<T>> TapError<T>(
        this Task<Result<T>> resultTask,
        Action<Error> action)
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            action(result.TopError);
        }

        return result;
    }

    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Func<T, Error> errorFactory)
    {
        if (result.IsFailure) return result;

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(errorFactory(result.Value));
    }

    public static async Task<Result<T>> Ensure<T>(
        this Task<Result<T>> resultTask,
        Func<T, bool> predicate,
        Func<T, Error> errorFactory) // This is the missing piece
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(errorFactory(result.Value));
    }

// 1. Ensure for non-generic Result
    public static Result Ensure(this Result result, Func<Unit, bool> predicate, Error error)
    {
        if (result.IsFailure) return result;
        return predicate(Unit.Value) ? result : Result.Failure(error);
    }

    // 2. Bind for non-generic Result (Result -> Result)
    public static Result Bind(this Result result, Func<Unit, Result> func)
    {
        if (result.IsFailure) return result;
        return func(Unit.Value);
    }

    // 3. Tap for non-generic Result
    public static Result Tap(this Result result, Action<Unit> action)
    {
        if (result.IsSuccess)
        {
            action(Unit.Value);
        }
        return result;
    }

    // 4. Map from non-generic Result to Result<T> (e.g., to Unit.Value)
    public static Result<T> Map<T>(this Result result, Func<Unit, T> func)
    {
        if (result.IsFailure) return Result.Failure<T>(result.TopError);
        return Result.Success(func(Unit.Value));
    }

    public static Result<T> Tap<T>(this Result<T> result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }
        return result;
    }

public static async Task<Result<T>> Ensure<T>(
    this Task<Result<T>> resultTask,
    Func<T, Task<Result>> predicate) // 👈 This handles the async check
{
    var result = await resultTask;

    if (result.IsFailure) return result;

    var ensureResult = await predicate(result.Value);
    
    return ensureResult.IsFailure ? Result.Failure<T>(ensureResult.TopError) : result;
}


}