using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.Table
{
    public class DeleteCmd : BaseCqrsCommand, ICommand
    {
        public DeleteCmd(Guid id, int originalVersion)
        {
            Id = id;
            ExpectedVersion = originalVersion;
        }
    }
}
