namespace Example.Api.Infrastructure.Services;

using System;
using Microsoft.AspNetCore.Http;

public class RequestHeaderAccessor(IHttpContextAccessor httpContextAccessor) : IRequestHeaderAccessor
{
    public string GetHeaderValue(string headerName)
    {
        ArgumentNullException.ThrowIfNull(headerName, nameof(headerName));

        var headers = httpContextAccessor.HttpContext?.Request?.Headers;

        ArgumentNullException.ThrowIfNull(headers, nameof(headers));

        return headers.TryGetValue(headerName, out var value)
            ? value.ToString()
            : string.Empty;
    }
}