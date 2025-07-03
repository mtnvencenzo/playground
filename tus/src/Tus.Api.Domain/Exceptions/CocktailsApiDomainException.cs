namespace Tus.Api.Domain.Exceptions;

/// <summary>Exception type for domain exceptions
/// </summary>
public class TusApiDomainException : Exception
{
    public TusApiDomainException() { }

    public TusApiDomainException(string message)
        : base(message) { }

    public TusApiDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
