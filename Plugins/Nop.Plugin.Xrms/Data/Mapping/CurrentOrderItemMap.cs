
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CurrentOrderItemMap : NopEntityTypeConfiguration<CurrentOrderItem>
    {
        #region Ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public CurrentOrderItemMap()
        {

        }
        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CurrentOrderItem> builder)
        {
            builder.ToTable("XrmsCurrentOrderItem");
            builder.HasKey(orderItem => orderItem.Id);
            builder.HasOne(orderItem => orderItem.CurrentOrder).WithMany(order => order.CurrentOrderItems).HasForeignKey(orderItem => orderItem.CurrentOrderId).IsRequired();
            builder.Ignore(orderItem => orderItem.State);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}
