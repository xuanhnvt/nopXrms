using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder
{
    public class DeletedEvent : BaseCqrsEvent
    {
        public DeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}
