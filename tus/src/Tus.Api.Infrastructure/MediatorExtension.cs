namespace Tus.Api.Infrastructure;

using MediatR;

internal static class MediatorExtension
{
#pragma warning disable IDE0060 // Remove unused parameter
    public static Task DispatchDomainEventsAsync(this IMediator mediator) => Task.CompletedTask;
}
