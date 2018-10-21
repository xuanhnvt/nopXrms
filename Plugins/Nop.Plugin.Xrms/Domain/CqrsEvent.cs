using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a cqrs event
    /// </summary>
    public partial class CqrsEvent : BaseEntity
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
    }
}
