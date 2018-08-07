using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class OrderItemNoteMap : NopEntityTypeConfiguration<OrderItemNote>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderItemNoteMap()
        {

        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<OrderItemNote> builder)
        {
            builder.ToTable("XrmsOrderItemNote");
            builder.HasKey(s => s.Id);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}