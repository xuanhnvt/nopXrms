using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using CQRSlite.Events;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Table;
using Nop.Plugin.Xrms.Services;

using TableEntity = Nop.Plugin.Xrms.Domain.Table;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Handlers
{
    public class TableEventHandlers : ICancellableEventHandler<CreatedEvent>,
                                      ICancellableEventHandler<DeletedEvent>,
                                      ICancellableEventHandler<EdittedEvent>
    {
        private readonly ITableService _tableService;

        public TableEventHandlers(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task Handle(CreatedEvent message, CancellationToken token)
        {
            _tableService.InsertTable(new TableEntity()
            {
                AggregateId = message.Id,
                Version = message.Version,
                //Code = message.Code,
                Name = message.Name,
                //Capacity = message.Capacity,
                //AreaId = message.LocationId,
                CreatedOnUtc = message.TimeStamp.UtcDateTime,
                UpdatedOnUtc = message.TimeStamp.UtcDateTime,
                Description = message.Description

            });
            return Task.CompletedTask;
        }

        public Task Handle(EdittedEvent message, CancellationToken token)
        {
            var table = _tableService.GetTableByAggregateId(message.Id);
            table.Version = message.Version;
            //table.Code = message.NewCode;
            table.Name = message.NewName;
            //table.Capacity = message.NewCapacity;
            //table.AreaId = message.NewLocationId;
            table.Description = message.NewDescription;
            table.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _tableService.UpdateTable(table);
            return Task.CompletedTask;
        }

        public Task Handle(DeletedEvent message, CancellationToken token)
        {
            var table = _tableService.GetTableByAggregateId(message.Id);
            _tableService.DeleteTable(table);
            return Task.CompletedTask;
        }
    }
}
