using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a order-table mapping
    /// </summary>
    public partial class OrderTableMapping : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the table identifier
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets a order code
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Gets or sets a table
        /// </summary>
        public virtual Table Table { get; set; }
    }
}
