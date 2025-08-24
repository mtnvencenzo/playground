namespace Example.Api.Infrastructure.Services;

public interface IRequestHeaderAccessor
{
    string GetHeaderValue(string headerName);
}
