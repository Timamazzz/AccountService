namespace AccountService.Core.Errors.Interfaces;

/// <summary>
/// Base interface for error definitions.
/// </summary>
public interface IErrorDefinition
{
    /// <summary> Unique error code (string format). </summary>
    string Code { get; }

    /// <summary> Short title describing the error. </summary>
    string Title { get; }

    /// <summary> Detailed error description. </summary>
    string Detail { get; }

    /// <summary> More in-depth explanation (used for API documentation). </summary>
    string Description { get; }
}