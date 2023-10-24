using DevShop.Core.DomainObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Core.Mediator;

public static class MediatorExtension
{
    public static async Task PublishEvent<T>(this IMediator mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notifications)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearEvent());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
            {
                await mediator.Publish(domainEvent);
            });
        await Task.WhenAll(tasks);
    }
}