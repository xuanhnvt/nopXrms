using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs
{
    public abstract class BaseCqrsCommand
    {
        /// <summary>
        /// The Aggregate ID of the Aggregate Root being changed
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Expected Version which the Aggregate will become.
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}
