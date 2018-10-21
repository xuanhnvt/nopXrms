using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Table
{
    public class EdittedEvent : BaseCqrsEvent
    {
        public readonly string NewCode;
        public readonly string NewName;
        public readonly byte NewCapacity;
        public readonly Guid NewLocationId;
        public readonly string NewDescription;

        public EdittedEvent(Guid id, string newCode, string newName, byte newCapacity, Guid newLocationId, string newDescription)
        {
            Id = id;
            NewCode = newCode;
            NewName = newName;
            NewCapacity = newCapacity;
            NewLocationId = newLocationId;
            NewDescription = newDescription;
        }
    }
}
