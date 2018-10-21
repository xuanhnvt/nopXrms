using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;
using Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder
{
    public class UpdateOrderItemsCmd : BaseCqrsCommand, ICommand
    {
        public readonly UpdateCurrentOrderItemsModel CommandModel;

        public UpdateOrderItemsCmd(Guid id, int originalVersion, UpdateCurrentOrderItemsModel model)
        {
            Id = id;
            ExpectedVersion = originalVersion;
            CommandModel = model ?? throw new ArgumentNullException();
        }
    }
}
