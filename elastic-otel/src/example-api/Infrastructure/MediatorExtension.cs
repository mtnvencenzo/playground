namespace Example.Api.Infrastructure;

using Example.Api.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

internal static class MediatorExtension
{
    public async static Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        var eventEmitters = ctx.ChangeTracker
            .Entries<IEventEmitter<IEvent>>()
            .Where(x => x.Entity.DomainEvents.Count > 0);

        if (!eventEmitters.Any())
        {
            return;
        }

        foreach (var evt in eventEmitters.SelectMany(x => x.Entity.DomainEvents))
        {
            await mediator.Publish(evt);
        }
    }
}
