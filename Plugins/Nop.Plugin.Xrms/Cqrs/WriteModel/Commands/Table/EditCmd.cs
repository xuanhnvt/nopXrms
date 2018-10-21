using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.Table
{
    public class EditCmd : BaseCqrsCommand, ICommand
    {
        public readonly string NewCode;
        public readonly string NewName;
        public readonly byte NewCapacity;
        public readonly Guid NewLocationId;
        public readonly string NewDescription;

        public EditCmd(Guid id, int originalVersion, string newCode, string newName, byte newCapacity, Guid newLocationId, string newDescription)
        {
            Id = id;
            ExpectedVersion = originalVersion;
            NewCode = newCode;
            NewName = newName;
            NewCapacity = newCapacity;
            NewLocationId = newLocationId;
            NewDescription = newDescription;
        }
    }
}
