using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using CQRSlite.Events;
using Newtonsoft.Json;

using Nop.Core.Data;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel
{
    /// <summary>
    /// Represents a material group
    /// </summary>
    public class CqrsEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly IRepository<CqrsEvent> _cqrsEventReposiory;
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();

        public CqrsEventStore(IEventPublisher publisher, IRepository<CqrsEvent> cqrsEventReposiory)
        {
            _publisher = publisher;
            _cqrsEventReposiory = cqrsEventReposiory;
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var @event in events)
            {
                CqrsEvent entity = new CqrsEvent()
                {
                    AggregateId = @event.Id,
                    Version = @event.Version,
                    TimeStamp = @event.TimeStamp,
                    Data = JsonConvert.SerializeObject(@event),
                    EventType = @event.GetType().ToString()
                };

                _cqrsEventReposiory.Insert(entity);

                await _publisher.Publish(@event, cancellationToken);
            }
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            /*IEnumerable<IEvent> events = _cqrsEventReposiory.Table.Where(e => e.AggregateId == aggregateId)
                .Select(o => (IEvent) JsonConvert.DeserializeObject(o.Data, Type.GetType(o.EventType)));*/
            // cause above code not work (don't know why), use following code
            List<IEvent> events = new List<IEvent>();
            foreach (CqrsEvent item in _cqrsEventReposiory.Table.Where(e => e.AggregateId == aggregateId).ToList())
            {
                IEvent @event = (IEvent) JsonConvert.DeserializeObject(item.Data, Type.GetType(item.EventType));
                events.Add(@event);
            }
            return Task.FromResult(events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>());
        }
    }
}
