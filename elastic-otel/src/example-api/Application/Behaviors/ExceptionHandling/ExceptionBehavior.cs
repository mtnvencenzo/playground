namespace Example.Api.Application.Behaviors.ExceptionHandling;

using System;
using System.Threading.Tasks;
using Example.Api.Application.Exceptions;
using Example.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class ExceptionBehavior
{
    public async static Task OnException(HttpContext context, Exception ex)
    {
        var requestId = context.TraceIdentifier ?? Guid.NewGuid().ToString();
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var logger = context.RequestServices.GetService<ILogger<Program>>();

        if (ex is OperationCanceledException)
        {
            // Issuing non-standard status code when the client 
            // has disconnected (This is based off of nginx)
            context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            context.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Client Closed Request";
            logger?.LogWarning(ex, "The client disconnected");
        }
        else if (ex is ExampleApiValidationException validationError)
        {
            var statusCode = validationError.GetSuggestedHttpStatusCode();

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors has occurred"
            };

            if (validationError.Errors is not null)
            {
                problemDetails.Extensions["errors"] = validationError.Errors;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problemDetails);

            logger.LogWarning("Validation errors {@ValidationErrors}", validationError.Errors);

        }
        else if (ex is ExampleApiDomainException)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "DomainException",
                Title = "Domain exception",
                Detail = string.Format("A domain error has occured: {0}", requestId)
            };

            await context.Response.WriteAsJsonAsync(problemDetails);

            logger?.LogError(ex, "An unhandled domain exception occured");
        }
        else
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "UnhandledException",
                Title = "Unhandled exception",
                Detail = string.Format("An unhandled error has occured: {0}", requestId)
            };

            await context.Response.WriteAsJsonAsync(problemDetails);

            logger?.LogError(ex, "An unhandled exception occured");
        }
    }
}
