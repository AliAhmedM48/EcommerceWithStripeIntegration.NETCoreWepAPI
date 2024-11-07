namespace demo.Errors;

public class ApiExceptionResponse : ApiErrorResponse
{
    public string? Details { get; set; }
    public ApiExceptionResponse(string? message = null, string? details = null) : base(500, message)
    {
        Details = details;
    }
}
