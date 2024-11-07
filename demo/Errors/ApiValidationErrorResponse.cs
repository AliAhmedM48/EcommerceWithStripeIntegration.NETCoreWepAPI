namespace demo.Errors;

public class ApiValidationErrorResponse : ApiErrorResponse
{
    public IEnumerable<string> Errors { get; set; } = new List<string>();
    public ApiValidationErrorResponse(string? message = null) : base(400, message)
    {
    }
}
