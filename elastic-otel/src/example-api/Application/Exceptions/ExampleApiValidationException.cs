namespace Example.Api.Application.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

public class ExampleApiValidationException : Exception
{
    public ExampleApiValidationException() { }

    public ExampleApiValidationException(string message)
        : base(message) { }

    public ExampleApiValidationException(string message, Exception innerException)
        : base(message, innerException) { }

    public ExampleApiValidationException(List<ValidationFailure> errors)
    {
        this.Errors = errors;
    }

    public List<ValidationFailure> Errors { get; init; }

    public int GetSuggestedHttpStatusCode()
    {
        if (this.Errors == null || this.Errors.Count == 0)
        {
            return StatusCodes.Status400BadRequest;
        }

        var statusCodes = new List<int>();

        this.Errors
            .Where(x => !string.IsNullOrWhiteSpace(x.ErrorCode))
            .Select(x => x.ErrorCode)
            .Distinct()
            .ToList()
            .ForEach(x =>
            {
                if (int.TryParse(x, out var statusCode))
                {
                    statusCodes.Add(statusCode);
                }
            });

        if (statusCodes.Count > 0)
        {
            return statusCodes.OrderByDescending(x => x).First();
        }

        return StatusCodes.Status400BadRequest;
    }
}
