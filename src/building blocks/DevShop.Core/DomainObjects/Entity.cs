﻿using System.ComponentModel.DataAnnotations;
using DevShop.Core.Messages;

namespace DevShop.Core.DomainObjects;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private List<Event> _notifications;
    public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public void AddEvent(Event @event)
    {
        _notifications = _notifications ?? new List<Event>(); 
        _notifications.Add(@event);
    }

    public void RemoveEvent(Event @event)
    {
        _notifications?.Remove(@event);
    }

    public void ClearEvent()
    {
        _notifications?.Clear();
    }

    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }

    #region Comparações

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    #endregion

}