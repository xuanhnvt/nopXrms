using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Table
{
    public class CreatedEvent : BaseCqrsEvent
    {
        public readonly string Code;
        public readonly string Name;
        public readonly byte Capacity;
        public readonly Guid LocationId;
        public readonly string Description;

        public CreatedEvent(Guid id, string code, string name, byte capacity, Guid locationId, string description)
        {
            Id = id;
            Code = code;
            Name = name;
            Capacity = capacity;
            LocationId = locationId;
            Description = description;
        }
    }
}
