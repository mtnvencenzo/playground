namespace Example.Api.Domain.Exceptions;

using System;

/// <summary>Exception type for domain exceptions
/// </summary>
public class ExampleApiDomainException : Exception
{
    public ExampleApiDomainException() { }

    public ExampleApiDomainException(string message)
        : base(message) { }

    public ExampleApiDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
