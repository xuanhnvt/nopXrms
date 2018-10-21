using Nop.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    /// <summary>
    /// For add and update item of order
    /// </summary>
    public partial class OrderItemModel : BaseNopModel
    {
        /// <summary>
        /// Id = 0: for add command, Id != 0: for update command
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// no use in add command
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// no use in add command
        /// </summary>
        public int Version { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
