using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class SupplierMap : NopEntityTypeConfiguration<Supplier>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public SupplierMap()
        {
            //this.ToTable("XrmsSupplier");
            //this.HasKey(s => s.Id);
            //this.Property(s => s.Name).IsRequired().HasMaxLength(400);
        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("XrmsSupplier");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(400);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}