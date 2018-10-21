using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CQRSlite.Domain;
using CQRSlite.Commands;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.Table;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Domain;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Handlers
{
    public class TableCommandHandlers: ICommandHandler<CreateCmd>,
                                       ICancellableCommandHandler<EditCmd>,
                                       ICancellableCommandHandler<DeleteCmd>
    {
        private readonly ISession _session;

        public TableCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateCmd message)
        {
            var item = new Table(message);
            await _session.Add(item);
            await _session.Commit();
        }

        public async Task Handle(DeleteCmd message, CancellationToken token)
        {
            var item = await _session.Get<Table>(message.Id, message.ExpectedVersion, token);
            item.Delete();
            await _session.Commit(token);
        }

        public async Task Handle(EditCmd message, CancellationToken token)
        {
            var item = await _session.Get<Table>(message.Id, message.ExpectedVersion, token);
            item.Edit(message);
            await _session.Commit(token);
        }
    }
}
