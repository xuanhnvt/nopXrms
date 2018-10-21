using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;
using Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder
{
    public class CreateCmd : BaseCqrsCommand, ICommand
    {
        public readonly CreateCurrentOrderModel CommandModel;

        public CreateCmd(Guid id, CreateCurrentOrderModel model)
        {
            Id = id;
            CommandModel = model ?? throw new ArgumentNullException();
        }
    }
}
