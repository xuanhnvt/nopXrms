using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents a restaurant table
    /// </summary>
    public partial class Table : BaseEntity, ILocalizedEntity
    {
        private ICollection<OrderTableMapping> _orderTableMappings;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

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
        /// Gets or sets the current order on this table
        /// </summary>
        public virtual CurrentOrder CurrentOrder { get; set; }

        /// <summary>
        /// Gets or sets the collection of order table mappings
        /// </summary>
        public virtual ICollection<OrderTableMapping> OrderTableMappings
        {
            get { return _orderTableMappings ?? (_orderTableMappings = new List<OrderTableMapping>()); }
            protected set { _orderTableMappings = value; }
        }
    }
}
