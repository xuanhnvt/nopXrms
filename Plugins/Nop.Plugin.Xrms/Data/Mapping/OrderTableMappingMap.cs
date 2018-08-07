
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class OrderTableMappingMap : NopEntityTypeConfiguration<OrderTableMapping>
    {
        #region Ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public OrderTableMappingMap()
        {

        }
        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<OrderTableMapping> builder)
        {
            builder.ToTable("XrmsOrderTableMapping");
            builder.HasKey(m => m.Id);
            builder.HasOne(m => m.Table).WithMany(tbl => tbl.OrderTableMappings).HasForeignKey(m => m.TableId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}
