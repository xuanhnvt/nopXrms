using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Table
{
    public class DeletedEvent : BaseCqrsEvent
    {
        public DeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}
