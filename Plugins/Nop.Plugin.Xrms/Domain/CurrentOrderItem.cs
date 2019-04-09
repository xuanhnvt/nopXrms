using System;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents the current order status enumeration
    /// </summary>
    public enum CurrentOrderItemState
    {
        New = 0,
        Editted = 10,
        Processing = 20,
        Ready = 30,
        Served = 40,
        Cancelled = 100
    }

    /// <summary>
    /// Represents current order item
    /// </summary>
    public partial class CurrentOrderItem : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the aggregate id
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the current order id
        /// </summary>
        public int CurrentOrderId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart item identifier
        /// </summary>
        public int ShoppingCartItemId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (include tax)
        /// </summary>
        //public decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (exclude tax)
        /// </summary>
        //public decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (include tax)
        /// </summary>
        //public decimal PriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (exclude tax)
        /// </summary>
        //public decimal PriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (include tax)
        /// </summary>
        //public decimal DiscountAmountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (exclude tax)
        /// </summary>
        //public decimal DiscountAmountExclTax { get; set; }

        /// <summary>
        /// Gets or sets the date start processing product
        /// </summary>
        public DateTime? StartedProcessingOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date stop processing product
        /// </summary>
        public DateTime? StoppedProcessingOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date serving product
        /// </summary>
        public DateTime? ServedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the order state
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the product info
        /// </summary>
        //public Product Product { get; set; }

        /// <summary>
        /// Gets or sets the current order
        /// </summary>
        public virtual CurrentOrder CurrentOrder { get; set; }

        /// <summary>
        /// Get or set item state
        /// </summary>
        public CurrentOrderItemState State
        {
            get => (CurrentOrderItemState)StateId;
            set => StateId = (int)value;
        }

        /// <summary>
        /// Gets or sets the shopping cart item
        /// </summary>
        public virtual ShoppingCartItem ShoppingCartItem { get; set; }
    }
}
