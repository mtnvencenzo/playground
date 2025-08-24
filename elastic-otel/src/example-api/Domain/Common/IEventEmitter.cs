namespace Example.Api.Domain.Common;

using System.Collections.Generic;

public interface IEventEmitter<IEvent>
{
    void AddDomainEvent(IEvent eventItem);

    void RemoveDomainEvent(IEvent eventItem);

    void ClearDomainEvents();

    IReadOnlyCollection<IEvent> DomainEvents { get; }
}
