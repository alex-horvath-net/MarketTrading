using System.Data;
using System.Reflection;
using System.Text.Json;
using TradingService.Domain;
using TradingService.Infrastructure.Database;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.EventStore;

public class EventStore(TradingDbContext db) : IEventStore<Guid> {

    public IEnumerable<EventDescription<Guid>> GetEvents(Guid aggregateId) => db.Events
        .Where(e => e.Id == aggregateId)
        .OrderBy(e => e.SequenceNumber)
        .Select(ToEventDescription)
        .ToList();

    private List<EventDescription<Guid>> _newEvents = new();

    public void AppendEvent(EventDescription<Guid> eventDescription) {
        var eventNodel = FromEventDescription(eventDescription);
        db.Events.Add(eventNodel);
    }

    public void SaveChanges() {
       db.SaveChanges();
    }

    private EventModel FromEventDescription(EventDescription<Guid> eventDescription) => new() {
        Id = eventDescription.AggregateId,
        SequenceNumber = eventDescription.SequenceNumber,
        RaisedAt = eventDescription.RaisedAt,
        EventTypeName = eventDescription.BusinessEvent.GetType().FullName,
        EventContent = JsonSerializer.Serialize(eventDescription.BusinessEvent)
    };

    private EventDescription<Guid> ToEventDescription(EventModel eventModel) {

        if (eventModel.EventTypeName == null)
            throw new Exception("EventTypeName should not be null");

        var eventType = _domainAssembly.GetType(eventModel.EventTypeName);
        if (eventType == null)
            throw new Exception($"EventTypeName not Found: {eventModel.EventTypeName}");

        if (eventModel.EventContent == null)
            throw new Exception("EventContent should not be null");


        var businessEvent = JsonSerializer.Deserialize(eventModel.EventContent, eventType);
        if (businessEvent == null)
            throw new Exception($"{eventModel.EventTypeName} Businesss Event deserialization faled");

        return new(
            eventModel.Id,
            eventModel.SequenceNumber,
            eventModel.RaisedAt,
            businessEvent);
    }

    private static Assembly _domainAssembly = typeof(CommandRouter<Guid>).Assembly;
}
