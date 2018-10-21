using System;
using System.Collections.Generic;
using System.Text;

using CQRSlite.Commands;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.Table
{
    public class CreateCmd : BaseCqrsCommand, ICommand
    {

        public readonly string Code;
        public readonly string Name;
        public readonly byte Capacity;
        public readonly Guid LocationId;
        public readonly string Description;

        public CreateCmd(Guid id, string code, string name, byte capacity, Guid locationId, string description)
        {
            Id = id;
            Code = code;
            Name = name;
            Capacity = capacity;
            LocationId = locationId;
            Description = description;
        }
    }
}
