﻿using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder
{
    public class CreateCmd : BaseCqrsCommand, ICommand
    {
        public readonly CreateInStoreOrderModel CommandModel;

        public CreateCmd(Guid id, CreateInStoreOrderModel model)
        {
            Id = id;
            CommandModel = model ?? throw new ArgumentNullException();
        }
    }
}
