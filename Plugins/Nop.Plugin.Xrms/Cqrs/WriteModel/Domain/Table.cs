using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Domain;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Table;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.Table;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Domain
{
    public class Table : AggregateRoot
    {
        private bool _activated;

        #region Public Properties

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public byte Capacity { get; private set; }
        public byte State { get; private set; }
        public long CurrentOrderId { get; private set; }
        public Guid LocationId { get; private set; }

        #endregion

        public Table() { }

        public Table(CreateCmd command)
        {
            Id = command.Id;
            Code = command.Code;
            Name = command.Name;
            Capacity = command.Capacity;
            LocationId = command.LocationId;
            Description = command.Description;
            ApplyChange(new CreatedEvent(Id, Code, Name, Capacity, LocationId, Description));
        }

        private void Apply(CreatedEvent e)
        {
            _activated = true;
        }

        private void Apply(DeletedEvent e)
        {
            _activated = false;
        }


        private void Apply(EdittedEvent e)
        {

        }

        public void Edit(EditCmd command)
        {
            // check valid for command
            //if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            Id = command.Id;
            Code = command.NewCode;
            Name = command.NewName;
            Capacity = command.NewCapacity;
            LocationId = command.NewLocationId;
            Description = command.NewDescription;
            ApplyChange(new EdittedEvent(Id, Code, Name, Capacity, LocationId, Description));
        }

        public void Delete()
        {
            if (!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new DeletedEvent(Id));
        }
    }
}
