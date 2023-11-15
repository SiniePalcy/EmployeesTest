using Employees.Shared.Model;

namespace Employees.Shared.Responses;

[Serializable]

public class ErrorResponse
{
    public string Message { get; set; } = default!;
    public List<ErrorDto>? Errors { get; set; }
    public string StackTrace { get; set; } = default!;
}