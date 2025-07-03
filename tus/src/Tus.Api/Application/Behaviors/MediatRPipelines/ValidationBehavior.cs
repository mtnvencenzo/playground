namespace Tus.Api.Application.Behaviors.MediatRPipelines;

using Tus.Api.Application.Behaviors;
using Tus.Api.Application.Exceptions;
using Tus.Api.Application.Utilities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        logger.LogDebug("Validating command {CommandType}", typeName);

        var validationResults = new List<ValidationResult>();

        foreach (var validator in validators)
        {
            var result = validator.IsAsync()
                ? await validator.ValidateAsync(request, cancellationToken)
                : validator.Validate(request);

            validationResults.Add(result);
        }

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new TusApiValidationException(failures);
        }

        return await next();
    }
}

public static class IValidatorExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validator"></param>
    /// <returns></returns>
    public static bool IsAsync(this IValidator validator) => validator.GetType().GetInterface(nameof(IAsyncValidator)) != null;
}