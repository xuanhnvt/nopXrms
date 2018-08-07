using System;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a material group
    /// </summary>
    public partial class CurrentOrderItem : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the current order id
        /// </summary>
        public int CurrentOrderId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

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
        public decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (exclude tax)
        /// </summary>
        public decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (include tax)
        /// </summary>
        public decimal PriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (exclude tax)
        /// </summary>
        public decimal PriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (include tax)
        /// </summary>
        public decimal DiscountAmountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (exclude tax)
        /// </summary>
        public decimal DiscountAmountExclTax { get; set; }

        /// <summary>
        /// Gets or sets the date start processing product
        /// </summary>
        public DateTime? StartedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date stop processing product
        /// </summary>
        public DateTime? StoppedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date serving product
        /// </summary>
        public DateTime? ServedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the order state
        /// </summary>
        public int State { get; set; }

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
    }
}
