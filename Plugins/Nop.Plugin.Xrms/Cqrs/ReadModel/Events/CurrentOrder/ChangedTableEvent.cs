using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder
{
    public class ChangedTableEvent : BaseCqrsEvent
    {
        public int TableId { get; set; }

        public ChangedTableEvent(Guid id, int tableId)
        {
            Id = id;
            TableId = tableId;
        }
    }
}
