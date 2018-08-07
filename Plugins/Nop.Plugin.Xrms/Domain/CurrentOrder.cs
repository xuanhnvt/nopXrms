using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a material group
    /// </summary>
    public partial class CurrentOrder : BaseEntity, ILocalizedEntity
    {

        private ICollection<CurrentOrderItem> _currentOrderItems;

        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the table id
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets the order code
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Gets or sets the order state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the print count
        /// </summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// Gets or sets the lock state
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the date and time of billing order
        /// </summary>
        public DateTime BilledOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of checking out order
        /// </summary>
        public DateTime CheckedOutOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the restaurant table
        /// </summary>
        public virtual Table Table { get; set; }

        /// <summary>
        /// Gets or sets the order info
        /// </summary>
        //public virtual Order Order { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items
        /// </summary>
        public virtual ICollection<CurrentOrderItem> CurrentOrderItems
        {
            get { return _currentOrderItems ?? (_currentOrderItems = new List<CurrentOrderItem>()); }
            protected set { _currentOrderItems = value; }
        }
    }
}
