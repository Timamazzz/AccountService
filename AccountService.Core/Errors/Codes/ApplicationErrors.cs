using AccountService.Core.Errors.Base;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Codes;

/// <summary>
/// Predefined application errors (business logic and system failures).
/// </summary>
public static class ApplicationErrors
{
    /// <summary> The requested entity was not found. </summary>
    public static readonly IApplicationError NotFound = new ApplicationError(
        "NOT_FOUND",
        "Not Found",
        "The requested resource was not found.",
        404,
        "The specified entity does not exist or has been removed."
    );

    /// <summary> The entity is already taken (e.g., username, email, etc.). </summary>
    public static readonly IApplicationError AlreadyTaken = new ApplicationError(
        "ALREADY_TAKEN",
        "Already Taken",
        "The provided value is already in use.",
        409,
        "A unique constraint violation occurred. The value you provided is already taken."
    );

    /// <summary> Internal server error. </summary>
    public static readonly IApplicationError InternalServerError = new ApplicationError(
        "INTERNAL_SERVER_ERROR",
        "Internal Server Error",
        "An unexpected error occurred. Please try again later.",
        500,
        "A generic error message when something unexpected happens."
    );
}