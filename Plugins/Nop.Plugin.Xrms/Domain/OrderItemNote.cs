using System;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a order item change entry
    /// </summary>
    public partial class OrderItemNote : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the customer id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the item quantity adjustment
        /// </summary>
        public int QuantityAdjustment { get; set; }

        /// <summary>
        /// Gets or sets item quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets item state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets the order
        /// </summary>
        //public virtual Order Order { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        //public Product Product { get; set; }

        /// <summary>
        /// Gets the customter that do action
        /// </summary>
        //public Customer Customer { get; set; }
    }
}
