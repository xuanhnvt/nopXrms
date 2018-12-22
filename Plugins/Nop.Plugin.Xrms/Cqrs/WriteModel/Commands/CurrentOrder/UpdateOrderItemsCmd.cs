using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder
{
    public class UpdateOrderItemsCmd : BaseCqrsCommand, ICommand
    {
        public readonly UpdateInStoreOrderItemsModel CommandModel;

        public UpdateOrderItemsCmd(Guid id, int originalVersion, UpdateInStoreOrderItemsModel model)
        {
            Id = id;
            ExpectedVersion = originalVersion;
            CommandModel = model ?? throw new ArgumentNullException();
        }
    }
}
